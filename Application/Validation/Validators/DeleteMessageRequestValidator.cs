using Application.Interfaces;
using Application.Services.Interfaces;
using Application.Validation.Rules;
using Domain.Requests.Chat;
using FluentValidation;

namespace Application.Validation.Validators;

public class DeleteMessageRequestValidator : AbstractValidator<DeleteMessageRequest>
{
    public DeleteMessageRequestValidator(
        ICurrentUserService userService,
        IApplicationDbContext _dbContext)
    {
        RuleFor(x => x.MessageId)
            .MustAsync((messageId, cancellationToken) =>
                _dbContext.GetIsUserAuthorOfMessage(userService.UserId, messageId, cancellationToken));
    }
}