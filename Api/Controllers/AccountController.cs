using Api.Extensions;
using Application.Commands;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IMediator _mediator;

    public AccountController(IAccountService accountService, IMediator mediator)
    {
        _accountService = accountService;
        _mediator = mediator;
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

    [HttpPut]
    public async Task<IActionResult> UpdateAccountAsync(
        [FromBody] UpdateAccountRequest request,
        CancellationToken cancellationToken)
    {
        if (User.GetId() != request.UserId) return Forbid();

        var updateAccountCommand = new UpdateAccountCommand(request, cancellationToken);
        await _mediator.Send(updateAccountCommand, cancellationToken);

        return Ok();
    }

    [HttpGet]
    public async Task<Account?> GetCurrentAccountAsync(CancellationToken cancellationToken)
    {
        return await _accountService.GetAccountByIdAsync(User.GetId(), cancellationToken);
    }
}