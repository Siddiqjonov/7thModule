using ContactManager.Application.Dtos;
using ContactManager.Domain.Entities;

namespace ContactManager.Application.Interfaces;

public interface IUserRoleRepository
{
    Task<ICollection<UserRole>> SelectAllRolesAsync();
    Task<ICollection<User>> SelectAllUsersByRoleNameAsync(string roleName);
    Task<UserRole> SelectUserRoleByRoleName(string userRoleName);
    Task<long> InsertUserRoleAsync(UserRole userRole);
    Task<UserRole> SelectUserRoleByIdAsync(long userRoleId);
    Task DeleteUserRoleAsync(UserRole userRole);
    Task UpdateUserRoleAsync(UserRole userRole);
}
