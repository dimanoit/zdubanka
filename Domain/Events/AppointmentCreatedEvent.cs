using Domain.Common;
using MediatR;

namespace Domain.Events;

public class AppointmentCreatedEvent : INotification
{
    public AppointmentCreatedEvent(string appointmentId)
    {
        AppointmentId = appointmentId;
    }

    public string AppointmentId { get; }
}