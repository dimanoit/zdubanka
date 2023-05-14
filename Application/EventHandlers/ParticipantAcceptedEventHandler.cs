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

    // TODO optimize query 
    public async Task Handle(ParticipantAcceptedEvent notification, CancellationToken cancellationToken)
    {
        var eventEntity = await _applicationDbContext.Events
            .Include(ap => ap.EventParticipants)
            .FirstAsync(ap => ap.Id == notification.EventId, cancellationToken);

        var countOfPeople = eventEntity.EventLimitation.CountOfPeople;
        var alreadyAcceptedPeople = eventEntity.EventParticipants!.Count(ap =>
            ap.EventId == notification.EventId &&
            ap.Status == ParticipantStatus.Accepted
        );

        if (countOfPeople != alreadyAcceptedPeople) return;

        eventEntity.Status = EventStatus.Closed;
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}