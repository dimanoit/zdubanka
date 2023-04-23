using Api.Filters;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController, Route("api/[controller]"), Authorize]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<IActionResult> AddAccount([FromBody] Account account)
    {
        await _accountService.AddAsync(account);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountById(string id, CancellationToken cancellationToken = default)
    {
        var account = await _accountService.GetAccountByIdAsync(id, cancellationToken);
        if (account == null) return NotFound();
        return Ok(account);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAccount([FromBody] Account account)
    {
        await _accountService.UpdateAccountAsync(account);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(string id, CancellationToken cancellationToken)
    {
        var account = await _accountService.GetAccountByIdAsync(id, cancellationToken);
        if (account == null) return NotFound();

        await _accountService.DeleteAccountAsync(account);
        return Ok();
    }
}