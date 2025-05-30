using ContactManager.Application.Dtos;
using ContactManager.Application.Interfaces;
using ContactManager.Domain.Entities;
using ContactManager.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Infrastructure.Persistence.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly MainContext MainContext;

    public UserRoleRepository(MainContext mainContext)
    {
        MainContext = mainContext;
    }

    public async Task<long> InsertUserRoleAsync(UserRole userRole)
    {
        await MainContext.UserRoles.AddAsync(userRole);
        await MainContext.SaveChangesAsync();
        return userRole.UserRoleId;
    }

    public async Task<ICollection<UserRole>> SelectAllRolesAsync()
    {
        var userRoles = await MainContext.UserRoles.ToListAsync();
        return userRoles;
    }

    public async Task<ICollection<User>> SelectAllUsersByRoleNameAsync(string roleName)
    {
        var users = await MainContext.Users.Include(u => u.UserRole).Where(u => u.UserRole.UserRoleName == roleName).ToListAsync();
        return users;
    }

    public async Task<UserRole> SelectUserRoleByRoleName(string userRoleName)
    {
        var userRole = await MainContext.UserRoles.FirstOrDefaultAsync(uR => uR.UserRoleName == userRoleName);
        return userRole == null ? throw new EntityNotFoundException($"Role with role name: {userRoleName} not found") : userRole;
    }

    public async Task<UserRole> SelectUserRoleByIdAsync(long userRoleId)
    {
        var userRole = await MainContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserRoleId == userRoleId);
        return userRole ?? throw new NotFoundException($"User role with: {userRoleId} not found");
    }
    public async Task DeleteUserRoleAsync(UserRole userRole)
    {
        MainContext.UserRoles.Remove(userRole);
        await MainContext.SaveChangesAsync();
    }

    public async Task UpdateUserRoleAsync(UserRole userRole)
    {
        MainContext.UserRoles.Update(userRole);
        await MainContext.SaveChangesAsync();
    }
}
