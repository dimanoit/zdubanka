using Application.Interfaces;
using Domain.Enums;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.EventHandlers;

public class ParticipantAcceptedEventHandler : INotificationHandler<ParticipantAcceptedEvent>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public ParticipantAcceptedEventHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task Handle(ParticipantAcceptedEvent notification, CancellationToken cancellationToken)
    {
        var isNeedToCloseEvent = await _applicationDbContext.Events
            .Include(ap => ap.EventParticipants)
            .AnyAsync(ev =>
                    ev.EventParticipants != null &&
                    ev.Id == notification.EventId &&
                    ev.EventLimitation.CountOfPeople <=
                    ev.EventParticipants.Count(ap => ap.Status == ParticipantStatus.Accepted),
                cancellationToken);

        if (!isNeedToCloseEvent) return;

        await CloseEventAsync(notification.EventId, cancellationToken);
    }

    private async Task CloseEventAsync(string eventId, CancellationToken cancellationToken)
    {
        var eventEntity = await _applicationDbContext.Events
            .Where(ev => ev.Id == eventId)
            .FirstAsync(cancellationToken);

        eventEntity.Status = EventStatus.Closed;
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}