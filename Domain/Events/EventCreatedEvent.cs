using Domain.Common;
using MediatR;

namespace Domain.Events;

public class EventCreatedEvent : INotification
{
    public EventCreatedEvent(string eventId)
    {
        EventId = eventId;
    }

    public string EventId { get; }
}