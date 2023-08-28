using Api.Extensions;
using Api.Services;
using Application.Commands;
using Application.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Requests;
using Infrastructure.Services;
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
    private readonly IAzureBlobStorageService _azureBlobStorageService;

    public AccountController(IAccountService accountService, IMediator mediator, IAzureBlobStorageService azureBlobStorageService)
    {
        _accountService = accountService;
        _azureBlobStorageService = azureBlobStorageService;
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
    [HttpPut("update-photo")]
    public async Task<IActionResult> UpdateAccountPhotoAsync(
        string userId, IFormFile file, CancellationToken cancellationToken)
    {
        var user = await _accountService.GetAccountByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return NotFound($"User with ID {userId} not found.");
        }
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var uploadedPhotoBlobName = await _azureBlobStorageService.UploadFileAsync(file);

        user.ImageUrl = uploadedPhotoBlobName;

        await _accountService.UpdateAccountAsync(user);

        return Ok($"Account photo for user {userId} updated successfully.");
    }

    [HttpGet]
    public async Task<Account?> GetCurrentAccountAsync(CancellationToken cancellationToken)
    {
        return await _accountService.GetAccountByIdAsync(User.GetId(), cancellationToken);
    }
}