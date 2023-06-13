using Application.Interfaces;
using Application.Services.Interfaces;
using Application.Validation.Rules;
using Domain.Requests.Chat;
using FluentValidation;

namespace Application.Validation.Validators;

public class SendMessageRequestValidator : AbstractValidator<SendMessageRequest>
{
    public SendMessageRequestValidator(
        ICurrentUserService userService,
        IApplicationDbContext _dbContext)
    {
        RuleFor(x => x.ChatId)
            .MustAsync((chatId, cancellationToken) =>
                _dbContext.GetIsUserMemberOfChat(userService.UserId, chatId, cancellationToken));

        RuleFor(x => x.Content)
            .MaximumLength(1000)
            .MinimumLength(1);
    }
}