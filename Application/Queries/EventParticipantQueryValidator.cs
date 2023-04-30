using Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;

public class EventParticipantQueryValidator : AbstractValidator<EventParticipantQuery>
{
    public EventParticipantQueryValidator(IApplicationDbContext dbContext)
    {
        RuleFor(c => c.Request)
            .MustAsync(async (r, cancellation) => await IsEventBelongsToOrganizerAsync(dbContext, r.OrganizerId, r.EventId, cancellation))
            .OverridePropertyName(r => r.Request.OrganizerId)
            .WithMessage("User isn't organizer of this event");
    }

    private async Task<bool> IsEventBelongsToOrganizerAsync(
        IApplicationDbContext dbContext,
        string organizerId,
        string eventId,
        CancellationToken cancellationToken)
    {
        var isEventBelongsToOrganizer = await dbContext.Appointments
            .AsNoTracking()
            .AnyAsync(ap => ap.Id == eventId && ap.OrganizerId == organizerId, cancellationToken);

        return isEventBelongsToOrganizer;
    }
}