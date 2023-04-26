using FluentValidation;

namespace Application.Queries;

public class EventParticipantQueryValidator: AbstractValidator<EventParticipantQuery>
{
    public EventParticipantQueryValidator()
    {
        RuleFor(q => q.Request.EventId)
            .NotEmpty();
        
        RuleFor(q => q.Request.OrganizerId)
            .NotEmpty();
    }
}