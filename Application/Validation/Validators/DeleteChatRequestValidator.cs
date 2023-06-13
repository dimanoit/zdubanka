using Application.Interfaces;
using Application.Services.Interfaces;
using Application.Validation.Rules;
using Domain.Requests.Chat;
using FluentValidation;

namespace Application.Validation.Validators;

public class DeleteChatRequestValidator : AbstractValidator<DeleteChatRequest>
{
    public DeleteChatRequestValidator(
        ICurrentUserService userService,
        IApplicationDbContext _dbContext)
    {
        RuleFor(x => x.ChatId)
            .MustAsync((chatId, cancellationToken) =>
                _dbContext.GetIsUserMemberOfChat(userService.UserId, chatId, cancellationToken));
    }
}