﻿using ContactManager.Application.Dtos;

namespace ContactManager.Application.Services;
public interface IUserRoleService
{
    Task<ICollection<UserRoleDto>> GetAllRolesAsync();
    Task<ICollection<UserGetDto>> GetAllUsersByRoleNameAsync(string roleName);
    Task<long> AddUserRoleAsync(UserRoleCreateDto userRoleCreateDto, string userRoleName);
    Task DeleteUserRoleByIdAsync(long userRoleId, string userRoleName);
    Task UpdateUserRoleAsync(UserRoleDto userRoleDto);
}
