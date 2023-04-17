using Api.Mappers;
using Application.Interfaces;
using Domain.Response;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class SocialRegistrationController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly AppSettings _applicationSettings;

    public SocialRegistrationController(IAccountService accountService, IOptions<AppSettings> applicationSettings)
    {
        _accountService = accountService;
        _applicationSettings = applicationSettings.Value;
    }

    [HttpPost("google")]
    public async Task<AccountShort> LoginOrSignUpWithGoogle([FromBody] string credential)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new List<string> { _applicationSettings.GoogleClientId }
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

        var user = await _accountService.GetAccountByEmailAsync(payload.Email, default);

        if (user != null) return user.ToAccountShort();

        var account = payload.ToAccount();
        await _accountService.AddAsync(account);
        return account.ToAccountShort();
    }
}