using Api.Extensions;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(string id, CancellationToken cancellationToken)
    {
        if (User.GetId() != id) return Forbid();

        var account = await _accountService.GetAccountByIdAsync(id, cancellationToken);
        if (account == null) return NotFound();

        await _accountService.DeleteAccountAsync(account);
        return Ok();
    }
}