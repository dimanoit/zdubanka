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
        var isEventBelongsToOrganizer = await dbContext.AppointmentParticipants
            .Include(ap => ap.Appointment)
            .AnyAsync(ap => ap.Id == eventParticipantId && ap.Appointment.OrganizerId == organizerId, cancellationToken);

        return isEventBelongsToOrganizer;
    }
    
   
}