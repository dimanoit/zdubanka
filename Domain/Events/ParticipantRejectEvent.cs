using Domain.Common;

namespace Domain.Events;

public class ParticipantRejectedEvent : BaseEvent
{
    public ParticipantRejectedEvent(string userId, string appointmentId)
    {
        UserId = userId;
        AppointmentId = appointmentId;
    }

    public string UserId { get; }
    public string AppointmentId { get; }
}