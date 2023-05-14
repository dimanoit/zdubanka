using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Validation.Rules;

public static class EventParticipantCommonRules
{
    public static async Task<bool> IsEventBelongsToOrganizerAsync(
        this IApplicationDbContext dbContext,
        string organizerId,
        string eventParticipantId,
        CancellationToken cancellationToken)
    {
        var isEventBelongsToOrganizer = await dbContext.EventParticipants
            .Include(ap => ap.Event)
            .AnyAsync(ap => ap.Id == eventParticipantId && ap.Event.OrganizerId == organizerId, cancellationToken);

        return isEventBelongsToOrganizer;
    }
}