using ContactManager.Application.Dtos;
using ContactManager.Application.Interfaces;
using ContactManager.Domain.Errors;

namespace ContactManager.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepositroy UserRepositroy;

    public UserService(IUserRepositroy userRepositroy)
    {
        UserRepositroy = userRepositroy;
    }

    public async Task DeleteUserByUserIdAsync(long userId, string userRoleName)
    {
        if (userRoleName == "SuperAdmin")
        {
            await UserRepositroy.DeleteUserById(userId);
        }
        else if (userRoleName == "Admin")
        {
            var user = await UserRepositroy.SelectUserByIdAsync(userId);

            if (user.UserRole.UserRoleName == "User" && user.UserId == userId)
            {
                await UserRepositroy.DeleteUserById(userId);
            }
            else
            {
                throw new NotAllowedException("Admin can not delete admin");
            }
        }
        else
        {
            throw new ForbiddenException("Access forbidden to users");
        }
    }

    public async Task UpdateUserRoleAsync(long userId, long userRoleId, string userRoleName)
    {
        await (userRoleName == "SuperAdmin"
            ? UserRepositroy.UpdateUserRoleAsync(userId, userRoleId)
            : throw new NotAllowedException("Updating is not allowed for Users or Admin"));
    }

    public async Task<UserGetDto> GetUserByUserIdAsync(long userId)
    {
        var user = await UserRepositroy.SelectUserByIdAsync(userId);
        var userGetDto = new UserGetDto()
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            Role = user.UserRole.UserRoleName,
        };

        return userGetDto;
    }
}
