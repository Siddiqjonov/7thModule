using ContactManager.Application.Dtos;
using System.Security.Claims;

namespace ContactManager.Application.Helpers;

public interface ITokenService
{
    public string GenerateTokent(UserGetDto user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}