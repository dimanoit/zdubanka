using Application.Interfaces;
using Application.Mappers;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Requests;
using Domain.Response;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IApplicationDbContext _context;

    public AppointmentService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Appointment> CreateAsync(
        AppointmentCreationRequest appointmentRequest,
        string organizerId,
        CancellationToken cancellationToken)
    {
        var appointment = appointmentRequest.ToAppointment(organizerId);
        appointment.AddDomainEvent(new AppointmentCreatedEvent(appointment.Id));

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync(cancellationToken);
        return appointment;
    }

    public async Task ApplyOnAppointmentAsync(
        string appointmentId,
        string userId,
        CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments
            .Where(ap => ap.Id == appointmentId)
            .FirstOrDefaultAsync(cancellationToken);

        if (appointment == null) throw new ValidationException("There is no appointment with this id");

        var appointmentParticipant = new AppointmentParticipant
        {
            UserId = userId,
            AppointmentId = appointmentId
        };
        var participantAppliedEvent = new ParticipantAppliedEvent(userId, appointmentId);

        appointmentParticipant.AddDomainEvent(participantAppliedEvent);

        _context.AppointmentParticipants.Add(appointmentParticipant);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<AppointmentResponse> GetUsersAppointmentsAsync(
        AppointmentRetrieveRequest request,
        CancellationToken cancellationToken)
    {
        var baseQuery = _context.Appointments
            .AsNoTracking()
            .Where(ap => ap.OrganizerId == request.UserId)
            .Select(ap => ap.ToAppointmentResponseDto());

        var totalCount = await baseQuery.CountAsync(cancellationToken);
        var data = await baseQuery.Skip(request.Skip).Take(request.Take).ToArrayAsync(cancellationToken);

        return new AppointmentResponse
        {
            Data = data,
            TotalCount = totalCount
        };
    }
}