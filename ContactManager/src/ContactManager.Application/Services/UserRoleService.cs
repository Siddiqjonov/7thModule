using ContactManager.Application.Dtos;
using ContactManager.Application.Interfaces;
using ContactManager.Domain.Entities;
using ContactManager.Domain.Errors;

namespace ContactManager.Application.Services;

public class UserRoleService : IUserRoleService
{
    private readonly IUserRoleRepository UserRoleRepository;

    public UserRoleService(IUserRoleRepository userRoleRepository)
    {
        UserRoleRepository = userRoleRepository;
    }

    public async Task<ICollection<UserRoleDto>> GetAllRolesAsync()
    {
        var userRoels = await UserRoleRepository.SelectAllRolesAsync();

        var userRoleDto = userRoels.Select(userRole => ConverUserRoleToUserRoleDto(userRole)).ToList();
        return userRoleDto;
    }
    public async Task UpdateUserRoleAsync(UserRoleDto userRoleDto)
    {
        var userRole = await UserRoleRepository.SelectUserRoleByIdAsync(userRoleDto.UserRoleId);
        if (userRole is null) throw new NotFoundException($"User role with roleId: {userRoleDto.UserRoleId} not found");
        else
        {
            userRole.UserRoleName = userRoleDto.UserRoleName;
            userRole.Description = userRoleDto.Description;

            await UserRoleRepository.UpdateUserRoleAsync(userRole);
        }
    }

    public async Task<ICollection<UserGetDto>> GetAllUsersByRoleNameAsync(string roleName)
    {
        var users = await UserRoleRepository.SelectAllUsersByRoleNameAsync(roleName);

        var userRolesDto = users.Select(user => ConvertUserToUserGetDto(user)).ToList();
        return userRolesDto;
    }

    private UserRoleDto ConverUserRoleToUserRoleDto(UserRole userRole)
    {
        return new UserRoleDto()
        {
            UserRoleId = userRole.UserRoleId,
            UserRoleName = userRole.UserRoleName,
            Description = userRole.Description,
        };
    }

    private UserGetDto ConvertUserToUserGetDto(User user)
    {
        return new UserGetDto()
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.UserRole.UserRoleName,
        };
    }

    public async Task<long> AddUserRoleAsync(UserRoleCreateDto userRoleCreateDto, string userRoleName)
    {
        if (userRoleName != "SuperAdmin")
            throw new NotAllowedException("Access only allowed to SuperAdmin to add user roles");

        var userRole = new UserRole()
        {
            UserRoleName = userRoleCreateDto.UserRoleName,
            Description = userRoleCreateDto.Description,
        };

        var userRoleId = await UserRoleRepository.InsertUserRoleAsync(userRole);

        return userRoleId;
    }

    public async Task DeleteUserRoleByIdAsync(long userRoleId, string userRoleName)
    {
        if (userRoleName != "SuperAdmin")
            throw new NotAllowedException("Access denied for non SuperAdmins");

        var userRole = await UserRoleRepository.SelectUserRoleByIdAsync(userRoleId);
        await UserRoleRepository.DeleteUserRoleAsync(userRole);
    }
}
