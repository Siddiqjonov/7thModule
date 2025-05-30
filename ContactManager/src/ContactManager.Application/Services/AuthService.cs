using ContactManager.Application.Dtos;
using ContactManager.Application.Helpers.Security;
using ContactManager.Application.Helpers;
using ContactManager.Domain.Entities;
using ContactManager.Domain.Errors;
using ContactManager.Application.FluentValidation;
using ContactManager.Application.Interfaces;

namespace ContactManager.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepositroy UserRepositroy;
    private readonly ITokenService TokenService;
    private readonly IRefreshTokenRepository RefreshTokenRepository;
    private readonly IUserRoleRepository UserRoleRepository;

    public AuthService(IUserRepositroy userRepositroy, ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository)
    {
        UserRepositroy = userRepositroy;
        TokenService = tokenService;
        RefreshTokenRepository = refreshTokenRepository;
    }

    public async Task<LogInResponseDto> LoginUserAsync(UserLogInDto userLogInDto)
    {
        var logInValidator = new UserLogInDtoValidator();
        var result = logInValidator.Validate(userLogInDto);

        if (!result.IsValid)
        {
            var errors = "";
            foreach (var error in result.Errors)
            {
                errors = errors + "\n" + error.ErrorMessage;
            }
            throw new ValidationFailedException(errors);
        }

        var user = await UserRepositroy.SelectUserByUserNameAsync(userLogInDto.UserName);

        var checkUserPassword = PasswordHasher.Verify(userLogInDto.Password, user.Password, user.Salt);
        if (checkUserPassword == false)
        {
            throw new UnauthorizedException("User or password incorrect");
        }

        var userGetDto = new UserGetDto()
        {
            UserId = user.UserId,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.UserRole.UserRoleName,
        };

        var token = TokenService.GenerateTokent(userGetDto);
        var existingToken = await RefreshTokenRepository.SelectActiveTokenByUserIdAsync(user.UserId);

        var loginResponseDto = new LogInResponseDto()
        {
            AccessToken = token,
            TokenType = "Bearer",
            Expires = 24,
        };

        if (existingToken == null)
        {
            var refreshToken = TokenService.GenerateRefreshToken();
            var refreshTokenToDB = new RefreshToken()
            {
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(21),
                IsRevoked = false,
                UserId = user.UserId,
            };

            await RefreshTokenRepository.InsertRefreshTokenAsync(refreshTokenToDB);

            loginResponseDto.RefreshToken = refreshToken;
        }
        else
        {
            loginResponseDto.RefreshToken = existingToken.Token;
        }

        return loginResponseDto;
    }

    public async Task LogOutAsync(string token)
    {
        await RefreshTokenRepository.RemoveRefreshTokenAsync(token);
    }

    public async Task<LogInResponseDto> RefreshTokenAsync(RefreshRequestDto request)
    {
        var principal = TokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null) throw new ForbiddenException("Invalid Access token");

        var userClaim = principal.FindFirst(c => c.Type == "UserId");
        var userId = long.Parse(userClaim.Value);

        var refreshToken = await RefreshTokenRepository.SelectRefreshTokenAsync(request.RefreshToken, userId);
        if (refreshToken == null || refreshToken.Expires < DateTime.UtcNow || refreshToken.IsRevoked)
            throw new UnauthorizedAccessException("Invalid or expired refresh token");

        // make refresh token used
        refreshToken.IsRevoked = true;

        var user = await UserRepositroy.SelectUserByIdAsync(userId);

        var userGetDto = new UserGetDto()
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.UserRole.UserRoleName,
        };

        // issue new tokens
        var newAccessToken = TokenService.GenerateTokent(userGetDto);
        var newRefreshToken = TokenService.GenerateRefreshToken();

        var refreshTokenToDB = new RefreshToken()
        {
            Token = newRefreshToken,
            Expires = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.UserId,
        };

        await RefreshTokenRepository.InsertRefreshTokenAsync(refreshTokenToDB);

        return new LogInResponseDto()
        {
            AccessToken = newAccessToken,
            TokenType = "Bearer",
            RefreshToken = newRefreshToken,
            Expires = 900,
        };
    }

    public async Task<long> SignUpUserAsync(UserCreateDto userCreateDto)
    {
        var userValidator = new UserCreateDtoValidator();
        var result = userValidator.Validate(userCreateDto);


        if (!result.IsValid)
        {
            var errors = "";
            foreach (var error in result.Errors)
            {
                errors = errors + error.ErrorMessage;
            }
            throw new ValidationFailedException(errors);
        }

        var tupleFromHasher = PasswordHasher.Hasher(userCreateDto.Password);

        var userRoleName = "User";
        var userRoleOfUser = await UserRoleRepository.SelectUserRoleByRoleName(userRoleName);

        var user = new User()
        {
            FirstName = userCreateDto.FirstName,
            LastName = userCreateDto.LastName,
            UserName = userCreateDto.UserName,
            Email = userCreateDto.Email,
            PhoneNumber = userCreateDto.PhoneNumber,
            Password = tupleFromHasher.Hash,
            Salt = tupleFromHasher.Salt,
            UserRoleId = userRoleOfUser.UserRoleId,
        };

        return await UserRepositroy.InsertUserAsync(user);
    }
}
