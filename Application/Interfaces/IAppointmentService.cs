using Domain.Entities;
using Domain.Requests;
using Domain.Response;

namespace Application.Interfaces;

public interface IAppointmentService
{
    Task<Appointment> CreateAsync(
        AppointmentCreationRequest appointmentRequest,
        string organizerId,
        CancellationToken cancellationToken);

    public Task<AppointmentResponse> GetUsersAppointmentsAsync(
        AppointmentRetrieveRequest request,
        CancellationToken cancellationToken);

    Task<Appointment?> GetAsync(string appointmentId, CancellationToken cancellationToken);
    Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken);
    Task DeleteAsync(string appointmentId, CancellationToken cancellationToken);
    Task ApplyOnAppointmentAsync(string appointmentId, string userId, CancellationToken cancellationToken);
}