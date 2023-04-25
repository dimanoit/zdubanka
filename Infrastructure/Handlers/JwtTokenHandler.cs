using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Handlers;

public class JwtTokenHandler : ITokenHandler
{
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public JwtTokenHandler()
    {
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }

    public string WriteToken(SecurityToken token)
    {
        return _jwtSecurityTokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal ValidateToken(
        string token,
        TokenValidationParameters validationParameters,
        out SecurityToken validatedToken)
    {
        return _jwtSecurityTokenHandler.ValidateToken(token, validationParameters, out validatedToken);
    }
}