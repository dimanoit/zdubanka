using Domain.Common;

namespace Domain.Events;

public class ParticipantAcceptedEvent : BaseEvent
{
    public ParticipantAcceptedEvent(string userId, string appointmentId)
    {
        UserId = userId;
        AppointmentId = appointmentId;
    }

    public string UserId { get; }
    public string AppointmentId { get; }
}