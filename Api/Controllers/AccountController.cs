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

    // TODO check that User.GetId() == id
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(string id, CancellationToken cancellationToken)
    {
        var account = await _accountService.GetAccountByIdAsync(id, cancellationToken);
        if (account == null) return NotFound();

        await _accountService.DeleteAccountAsync(account);
        return Ok();
    }
}