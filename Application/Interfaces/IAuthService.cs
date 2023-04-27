using System.Security.Claims;
using Domain.Entities;
using Domain.Models;
using Domain.Response;

namespace Application.Interfaces;

public interface IAuthService
{
    public AuthenticationResponse GenerateToken(Account user);
    public string GenerateRefreshToken();
    public Result<ClaimsPrincipal?> GetPrincipalFromExpiredToken(string token);
}