using Api.Mappers;
using Application.Interfaces;
using Domain.Entities;
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
    private readonly UserManager<Account> _userManager;
    private readonly JwtService _jwtService;

    public AuthController(
        UserManager<Account> userManager,
        IAccountService accountService,
        IOptions<AppSettings> applicationSettings,
        JwtService jwtService)
    {
        _userManager = userManager;
        _accountService = accountService;
        _jwtService = jwtService;
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
        var identityUser = new Account()
        {
            Email = user.Email,
            FullName = user.Name,
            UserName = user.Email,
            Id = Guid.NewGuid().ToString(),
            Token = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(identityUser, user.Password);

        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }

        user.Password = null;
        return Results.Created("api/auth", user);
    }
    
    [HttpPost("token")]
    public async Task<ActionResult<AuthenticationResponse>> CreateBearerToken(AuthenticationRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null)
        {
            return BadRequest($"User with {request.UserName} hasn't registered");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
        {
            return BadRequest("Bad credentials");
        }

        var token = _jwtService.CreateToken(user);

        return Ok(token);
    }
}

public class UserRegistrationModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
}

public class AuthenticationRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class AuthenticationResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}