using Domain.Models;

namespace Domain.Requests;

public record AppointmentCreationRequest
{
    public Address Location { get; init; } = null!;
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public DateTime StartDay { get; init; }
    public DateTime EndDay { get; init; }
    public AppointmentLimitation AppointmentLimitation { get; init; } = null!;
}