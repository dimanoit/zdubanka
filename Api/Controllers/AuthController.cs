using Api.Mappers;
using Application.Interfaces;
using Domain.Entities;
using Domain.Requests;
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
    private readonly JwtService _jwtService;
    private readonly UserManager<Account> _userManager;

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
    public async Task<IResult> PostUser(RegistrationRequestModel user)
    {
        var identityUser = new Account
        {
            Email = user.Email,
            FullName = user.Name,
            UserName = user.Email,
            Id = Guid.NewGuid().ToString(),
        };

        var result = await _userManager.CreateAsync(identityUser, user.Password);

        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }

        user.Password = null;
        return Results.Created("api/auth", user);
    }

    [HttpPost, Route("refresh")]
    public async Task<ActionResult<AuthenticationResponse>> Refresh(
        RefreshTokenRequestModel refreshTokenRequestModel,
        CancellationToken cancellationToken)
    {
        string accessToken = refreshTokenRequestModel.AccessToken;
        string refreshToken = refreshTokenRequestModel.RefreshToken;
        var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
        var username = principal.Identity!.Name; //this is mapped to the Name claim by default

        var user = await _accountService.GetAccountByEmailAsync(username, default);
        
        
        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return BadRequest("Invalid client request");
        var newAccessToken = _jwtService.CreateToken(user);
        var newRefreshToken = _jwtService.GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        
        await _accountService.UpdateAccountAsync(user);

        var authenticationResponse = new AuthenticationResponse()
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

