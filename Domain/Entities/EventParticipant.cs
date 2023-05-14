using Domain.Common;
using Domain.Enums;
using Domain.Events;

namespace Domain.Entities;

public class EventParticipant : BaseEntity
{
    public string UserId { get; init; } = null!;
    public string EventId { get; init; } = null!;
    public ParticipantStatus Status { get; private set; } = ParticipantStatus.InReview;

    public Event Event { get; set; } = null!;
    public Account Account { get; set; } = null!;

    public void UpdateToAcceptStatus()
    {
        Status = ParticipantStatus.Accepted;
        var participantAcceptedEvent = new ParticipantAcceptedEvent(UserId, EventId);
        AddDomainEvent(participantAcceptedEvent);
    }

    public void UpdateToRejectStatus()
    {
        Status = ParticipantStatus.Rejected;
        var participantRejectedEvent = new ParticipantRejectedEvent(UserId, EventId);
        AddDomainEvent(participantRejectedEvent);
    }
}