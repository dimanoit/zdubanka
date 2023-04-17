using Application.Interfaces;
using Application.Mappers;
using Domain.Entities;
using Domain.Events;
using Domain.Requests;
using Domain.Response;
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
        string organizerEmail,
        CancellationToken cancellationToken)
    {
        var organizerId = await GetUserIdAsync(organizerEmail, cancellationToken);
        
        var appointment = appointmentRequest.ToAppointment(organizerId);
        appointment.AddDomainEvent(new AppointmentCreatedEvent(appointment.Id));

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync(cancellationToken);
        return appointment;
    }

    public async Task<Appointment?> GetAsync(string appointmentId, CancellationToken cancellationToken)
    {
        return await _context.Appointments
            .AsNoTracking()
            .FirstOrDefaultAsync(ap => ap.Id == appointmentId, cancellationToken);
    }

    public async Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string appointmentId, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(ap => ap.Id == appointmentId, cancellationToken);

        if (appointment == null) return;

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<AppointmentResponse> GetUsersAppointmentsAsync(
       AppointmentRetrieveRequest request,
        CancellationToken cancellationToken)
    {
        var userId = await GetUserIdAsync(request.Email, cancellationToken);

        var baseQuery =  _context.Appointments
            .Where(ap => ap.OrganizerId == userId)
            .Select(ap => ap.ToAppointmentResponseDto());

        var totalCount = await baseQuery.CountAsync(cancellationToken);
        var data = await baseQuery.Skip(request.Skip).Take(request.Take).ToArrayAsync(cancellationToken);

        return new AppointmentResponse
        {
            Data = data,
            TotalCount = totalCount
        };
    }

    private async Task<string> GetUserIdAsync(string email, CancellationToken cancellationToken)
    {
        var userId = await _context.Accounts
            .Where(ac => ac.Email == email)
            .Select(ac => ac.Id)
            .FirstAsync(cancellationToken);
        
        return userId;
    }
}