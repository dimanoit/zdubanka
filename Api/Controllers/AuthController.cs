using Api.Mappers;
using Application.Interfaces;
using Domain.Response;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly AppSettings _applicationSettings;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthController(
        UserManager<IdentityUser> userManager,
        IAccountService accountService,
        IOptions<AppSettings> applicationSettings)
    {
        _userManager = userManager;
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

    [HttpPost]
    public async Task<IResult> PostUser(UserRegistrationModel user)
    {
        var identityUser = new IdentityUser()
        {
            Email = user.Email
        };

        var result = await _userManager.CreateAsync(identityUser, user.Password);

        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }

        user.Password = null;
        return Results.Created("api/auth", user);
    }
}

public class UserRegistrationModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}