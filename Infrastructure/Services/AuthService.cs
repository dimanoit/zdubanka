using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;
using Application.Providers.Interfaces;
using Domain.Entities;
using Domain.Response;
using Infrastructure.Handlers;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api;

public class AuthService : IAuthService
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ITokenHandler _tokenHandler;
    private readonly TokenOptions _tokenOptions;

    public AuthService(
        IOptions<TokenOptions> tokenOptions,
        IDateTimeProvider dateTimeProvider,
        ITokenHandler tokenHandler)
    {
        _dateTimeProvider = dateTimeProvider;
        _tokenHandler = tokenHandler;
        _tokenOptions = tokenOptions.Value;
    }

    private SymmetricSecurityKey Key => new(Encoding.UTF8.GetBytes(_tokenOptions.Key));

    public AuthenticationResponse GenerateToken(Account user)
    {
        var expiration = _dateTimeProvider.UtcNow.AddMinutes(_tokenOptions.ExpirationMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, _dateTimeProvider.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _tokenOptions.Issuer,
            _tokenOptions.Audience,
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

        var authenticationResponse = new AuthenticationResponse
        {
            Token = _tokenHandler.WriteToken(token),
            RefreshToken = GenerateRefreshToken(),
            Expiration = expiration
        };

        return authenticationResponse;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = Key,
            ValidateLifetime = false
        };

        var principal = _tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (
            jwtSecurityToken == null ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase)
        )
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}