using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Handlers;

public interface ITokenHandler
{
    public string WriteToken(SecurityToken token);

    public ClaimsPrincipal? ValidateToken(
        string token,
        TokenValidationParameters validationParameters,
        out SecurityToken validatedToken);
}