using Domain.Entities;
using Domain.Requests;
using Domain.Response;

namespace Application.Services.Interfaces;

public interface IAppointmentService
{
    Task<Appointment> CreateAsync(
        AppointmentCreationRequest appointmentRequest,
        string organizerId,
        CancellationToken cancellationToken);

    public Task<AppointmentResponse> GetUsersAppointmentsAsync(
        AppointmentRetrieveRequest request,
        CancellationToken cancellationToken);

    Task ApplyOnAppointmentAsync(string appointmentId, string userId, CancellationToken cancellationToken);
}