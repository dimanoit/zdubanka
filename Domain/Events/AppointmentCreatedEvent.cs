using Domain.Common;

namespace Domain.Events;

public class AppointmentCreatedEvent : BaseEvent
{
    public AppointmentCreatedEvent(string appointmentId)
    {
        AppointmentId = appointmentId;
    }

    public string AppointmentId { get; }
}