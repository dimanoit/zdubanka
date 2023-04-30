using Domain.Common;
using Domain.Enums;
using Domain.Events;

namespace Domain.Entities;

public class AppointmentParticipant : BaseEntity
{
    public string UserId { get; init; } = null!;
    public string AppointmentId { get; init; } = null!;
    public ParticipantStatus Status { get; private set; } = ParticipantStatus.InReview;

    public Appointment Appointment { get; set; } = null!;
    public Account Account { get; set; } = null!;

    public void UpdateToAcceptStatus()
    {
        Status = ParticipantStatus.Accepted;
        var participantAcceptedEvent = new ParticipantAcceptedEvent(UserId, AppointmentId);
        AddDomainEvent(participantAcceptedEvent);
    }

    public void UpdateToRejectStatus()
    {
        Status = ParticipantStatus.Rejected;
        var participantRejectedEvent = new ParticipantRejectedEvent(UserId, AppointmentId);
        AddDomainEvent(participantRejectedEvent);
    }
}