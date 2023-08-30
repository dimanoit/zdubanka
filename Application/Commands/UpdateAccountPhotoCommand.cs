using Application.Interfaces;
using Application.Services.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands;

public record UpdateAccountPhotoCommand(string userId, IFormFile File, CancellationToken CancellationToken) : IRequest;

public class UpdateAccountPhotoCommandHandler : IRequestHandler<UpdateAccountPhotoCommand>
{
    private readonly IAccountService _accountService;
    private readonly IFileService _fileService;

    public UpdateAccountPhotoCommandHandler(IAccountService accountService, IFileService fileService)
    {
        _accountService = accountService;
        _fileService = fileService;
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

        var uploadedPhotoBlobName = await _fileService.UploadFileAsync(command.File);

        user.ImageUrl = uploadedPhotoBlobName;

        await _accountService.UpdateAccountAsync(user);
    }
}



