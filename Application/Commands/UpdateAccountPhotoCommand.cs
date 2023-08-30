using Application.Interfaces;
using Application.Services.Interfaces;
using Domain.Exceptions;
using Domain.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands;

public record UpdateAccountPhotoCommand(string userId, IFormFile File, CancellationToken CancellationToken) : IRequest;

public class UpdateAccountPhotoCommandHandler : IRequestHandler<UpdateAccountPhotoCommand>
{
    private readonly IAccountService _accountService;
    private readonly IAzureBlobStorageService _azureBlobStorageService;

    public UpdateAccountPhotoCommandHandler(IAccountService accountService, IAzureBlobStorageService azureBlobStorageService)
    {
        _accountService = accountService;
        _azureBlobStorageService = azureBlobStorageService;
    }

    public async Task Handle(UpdateAccountPhotoCommand command, CancellationToken cancellationToken)
    {
        var user = await _accountService.GetAccountByIdAsync(command.userId, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {command.userId} not found.");
        }
        if (command.File == null || command.File.Length == 0)
        {
            throw new ArgumentException("No file uploaded.");
        }

        var uploadedPhotoBlobName = await _azureBlobStorageService.UploadFileAsync(command.File);

        user.ImageUrl = uploadedPhotoBlobName;

        await _accountService.UpdateAccountAsync(user);
    }
}



