using Domain.Entities;
using Domain.Requests;
using Domain.Response;

namespace Application.Mappers;

public static class AppointmentMapper
{
    public static Appointment ToAppointment(this AppointmentCreationRequest request, string organizerId)
    {
        var appointment = new Appointment
        {
            Id = Guid.NewGuid().ToString(),
            Location = request.Location,
            Title = request.Title,
            Description = request.Description,
            StartDay = request.StartDay,
            EndDay = request.EndDay,
            AppointmentLimitation = request.AppointmentLimitation,
            OrganizerId = organizerId
        };

        return appointment;
    }


    public static AppointmentResponseDto ToAppointmentResponseDto(this Appointment entity)
    {
        var appointment = new AppointmentResponseDto
        {
            Location = entity.Location,
            Title = entity.Title,
            Description = entity.Description,
            StartDay = entity.StartDay,
            EndDay = entity.EndDay,
            AppointmentLimitation = entity.AppointmentLimitation,
            OrganizerId = entity.OrganizerId,
            Id = entity.Id,
            Status = entity.Status
        };

        return appointment;
    }
}