using Application.Commands;
using Application.Interfaces;
using Application.Validation.Rules;
using FluentValidation;

namespace Application.Validation.Validators;

public class RejectEventParticipantCommandValidator: AbstractValidator<RejectEventParticipantCommand>
{
    public RejectEventParticipantCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(c => c.Request)
            .MustAsync(async (r, cancellation) =>
                await dbContext.IsEventBelongsToOrganizerAsync(r.OrganizerId, r.EventParticipantId, cancellation))
            .OverridePropertyName(r => r.Request.OrganizerId);
    }
}