using Application.Interfaces;
using Application.Services.Interfaces;
using Application.Validation.Rules;
using Domain.Requests.Chat;
using FluentValidation;

namespace Application.Validation.Validators;

public class UpdateChatRequestValidator : AbstractValidator<UpdateChatRequest>
{
    public UpdateChatRequestValidator(
        ICurrentUserService userService,
        IApplicationDbContext _dbContext)
    {
        RuleFor(x => x.ChatId)
            .MustAsync((chatId, cancellationToken) =>
                _dbContext.GetIsUserMemberOfChat(userService.UserId, chatId, cancellationToken));

        RuleFor(x => x.Members)
            .Must(x => x.Length > 2);
    }
}