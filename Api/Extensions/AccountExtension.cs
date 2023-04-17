using Api.Mappers;
using Domain.Response;
using Google.Apis.Auth;

namespace Api.Extensions;

public static class AccountExtension
{
    public static async Task<AccountShort> GetCurrentAccountAsync(
        this HttpContext context,
        AppSettings settings)
    {
        var token = context.Request.Headers["X-Google-Token"];
        var validationSettings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new List<string> { settings.GoogleClientId }
        };
        
        var payload = await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);

        return payload.ToAccountShort();
    }
}

