using Domain.Common;
using MediatR;

namespace Domain.Events;

public class ParticipantRejectedEvent : INotification
{
    public ParticipantRejectedEvent(string userId, string eventId)
    {
        UserId = userId;
        EventId = eventId;
    }

    public string UserId { get; }
    public string EventId { get; }
}