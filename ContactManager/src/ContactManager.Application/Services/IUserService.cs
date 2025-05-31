using ContactManager.Application.Dtos;

namespace ContactManager.Application.Services;

public interface IUserService
{
    Task DeleteUserByUserIdAsync(long userId, string userRoleName);
    Task UpdateUserRoleAsync(long userId, long userRoleId, string userRoleName);
    Task<UserGetDto> GetUserByUserIdAsync(long userId);
}
