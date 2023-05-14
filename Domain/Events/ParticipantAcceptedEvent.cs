using Domain.Common;
using MediatR;

namespace Domain.Events;

public class ParticipantAcceptedEvent : INotification
{
    public ParticipantAcceptedEvent(string userId, string eventId)
    {
        UserId = userId;
        EventId = eventId;
    }

    public string UserId { get; }
    public string EventId { get; }
}