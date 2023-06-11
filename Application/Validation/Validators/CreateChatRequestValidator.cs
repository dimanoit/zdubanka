using Domain.Requests.Chat;
using FluentValidation;

namespace Application.Validation.Validators;

public class CreateChatRequestValidator : AbstractValidator<CreateChatRequest>
{
    public CreateChatRequestValidator()
    {
        RuleFor(x => x.Members)
            .Must(x => x.Length > 2);
    }
}