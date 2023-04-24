using System.Security.Claims;
using Domain.Entities;
using Domain.Response;

namespace Application.Interfaces;

public interface IAuthService
{
    public AuthenticationResponse CreateToken(Account user);
    public string GenerateRefreshToken();
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}