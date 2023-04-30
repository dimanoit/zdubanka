using Domain.Common;
using MediatR;

namespace Domain.Events;

public class ParticipantAcceptedEvent : INotification
{
    public ParticipantAcceptedEvent(string userId, string appointmentId)
    {
        UserId = userId;
        AppointmentId = appointmentId;
    }

    public string UserId { get; }
    public string AppointmentId { get; }
}