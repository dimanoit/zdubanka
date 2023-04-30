using Domain.Common;
using MediatR;

namespace Domain.Events;

public class ParticipantRejectedEvent : INotification
{
    public ParticipantRejectedEvent(string userId, string appointmentId)
    {
        UserId = userId;
        AppointmentId = appointmentId;
    }

    public string UserId { get; }
    public string AppointmentId { get; }
}