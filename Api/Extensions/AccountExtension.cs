using Api.Mappers;
using Application.Interfaces;
using Domain.Response;
using Google.Apis.Auth;

namespace Api.Extensions;

public static class AccountExtension
{
    public static async Task<string> GetCurrentUserIdAsync(
        this HttpContext context,
        IAccountService accountService
        )
    {
        var token = context.Request.Headers["X-Token"];

        return await accountService.GetUserIdByTokenAsync(token);
    }
}

