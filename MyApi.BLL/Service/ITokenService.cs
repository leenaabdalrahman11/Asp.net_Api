using System;
using System.Security.Claims;
using MyApi.DAL.Models;

namespace MyApi.BLL.Service;

public interface ITokenService
{
    Task<string> GenerateAccessToken(ApplicationUser user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
