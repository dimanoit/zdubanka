using Api.Mappers;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Domain.Requests;
using Domain.Response;
using Google.Apis.Auth;
using Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly GoogleOptions _applicationSettings;
    private readonly AuthService _authService;
    private readonly UserManager<Account> _userManager;

    public AuthController(
        UserManager<Account> userManager,
        IAccountService accountService,
        IOptions<GoogleOptions> applicationSettings,
        AuthService authService)
    {
        _userManager = userManager;
        _accountService = accountService;
        _authService = authService;
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
    public async Task<IResult> PostUser(RegistrationRequestModel user)
    {
        var identityUser = new Account
        {
            Email = user.Email,
            FullName = user.Name,
            UserName = user.Email,
            Id = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(identityUser, user.Password);

        if (!result.Succeeded) return Results.BadRequest(result.Errors);

        user.Password = null;
        return Results.Created("api/auth", user);
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<ActionResult<AuthenticationResponse>> Refresh(
        RefreshTokenRequestModel refreshTokenRequestModel,
        CancellationToken cancellationToken)
    {
        var accessToken = refreshTokenRequestModel.AccessToken;
        var refreshToken = refreshTokenRequestModel.RefreshToken;
        var principal = _authService.GetPrincipalFromExpiredToken(accessToken);

        if (!principal.IsSuccess)
        {
            return BadRequest(principal.Exception);
        }
        
        var username = principal.Value.Identity!.Name; //this is mapped to the Name claim by default

        var user = await _accountService.GetAccountByEmailAsync(username, default);


        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return BadRequest("Invalid client request");
        var newAccessToken = _authService.GenerateToken(user);
        var newRefreshToken = _authService.GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;

        await _accountService.UpdateAccountAsync(user);

        var authenticationResponse = new AuthenticationResponse
        {
            Token = newAccessToken.Token,
            Expiration = newAccessToken.Expiration,
            RefreshToken = newRefreshToken
        };

        return Ok(authenticationResponse);
    }

    [HttpPost("token")]
    public async Task<ActionResult<AuthenticationResponse>> CreateBearerToken(AuthenticationRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null) return BadRequest($"User with {request.UserName} hasn't registered");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid) return BadRequest("Bad credentials");

        var token = _authService.GenerateToken(user);

        return Ok(token);
    }
}