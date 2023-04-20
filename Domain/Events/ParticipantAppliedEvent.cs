using Domain.Common;

namespace Domain.Events;

public class ParticipantAppliedEvent : BaseEvent
{
    public ParticipantAppliedEvent(string userId, string appointmentId)
    {
        UserId = userId;
        AppointmentId = appointmentId;
    }

    public string UserId { get; }
    public string AppointmentId { get; }
}