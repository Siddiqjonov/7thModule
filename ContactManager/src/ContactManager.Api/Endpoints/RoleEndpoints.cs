﻿using ContactManager.Application.Dtos;
using ContactManager.Application.Services;
using ContactManager.Domain.Errors;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ContactManager.Api.Endpoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/role")
            .RequireAuthorization()
            .WithTags("UserRole Management");

        userGroup.MapPost("/post", [Authorize(Roles = "SuperAdmin")]
        async (UserRoleCreateDto userRoleCreateDto, IUserRoleService _userRoleService, HttpContext context) =>
        {
            //var userRoleName = context.User.FindFirst("Role")?.Value;
            var userRoleName = context.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRoleName == null)
                throw new NotAllowedException("Access allowed to SuperAdmin!");
            var userRoleId = await _userRoleService.AddUserRoleAsync(userRoleCreateDto, userRoleName);
            return Results.Ok(userRoleId);
        })
        .WithName("AddUserRole");


        userGroup.MapGet("/getAll", [Authorize(Roles = "Admin, SuperAdmin")]
        async (IUserRoleService _userRoleService) =>
        {
            var roles = await _userRoleService.GetAllRolesAsync();
            return Results.Ok(roles);
        })
          .WithName("GetAllUsers");


        userGroup.MapDelete("/deleteById", [Authorize(Roles = "SuperAdmin")]
        async (HttpContext context, long userRoleId, IUserRoleService _userRoleService) =>
        {
            var userRoleName = context.User.FindFirst(ClaimTypes.Role)?.Value;
            //var userRoleName = context.User.FindFirst("Role")?.Value;
            if (userRoleName == null)
                throw new NotAllowedException("Access allowed to SuperAdmin!");

            await _userRoleService.DeleteUserRoleByIdAsync(userRoleId, userRoleName);
            return Results.Ok();
        })
            .WithName("DeleteUserRoleById");

        userGroup.MapPatch("/update", [Authorize(Roles = "SuperAdmin")]
        async (IUserRoleService _userRoleService, UserRoleDto userRoleDto) =>
        {
            if (userRoleDto == null)
            {
                return Results.BadRequest("User role data is required.");
            }

            try
            {
                await _userRoleService.UpdateUserRoleAsync(userRoleDto);
                return Results.Ok("User role updated successfully.");
            }
            catch (Exception ex)
            {
                // Optionally, log the exception here
                return Results.Problem($"An error occurred: {ex.Message}");
            }
        })
            .WithName("ChangeUserRole");
    }
}
