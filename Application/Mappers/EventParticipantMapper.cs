using Domain.Entities;
using Domain.Models;

namespace Application.Mappers;

public static class EventParticipantMapper
{
    public static EventParticipant ToEventParticipant(this AppointmentParticipant appointmentParticipant)
    {
        var eventParticipant = new EventParticipant
        {
            UserId = appointmentParticipant.UserId,
            UserName = appointmentParticipant.Account.FullName,
            AppointmentId = appointmentParticipant.AppointmentId,
            AppointmentTitle = appointmentParticipant.Appointment.Title,
            Status = appointmentParticipant.Status
        };
        
        return eventParticipant;
    }
}
