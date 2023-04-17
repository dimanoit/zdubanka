using Domain.Common;
using Domain.Models;

namespace Domain.Entities;

public class Appointment : BaseEntity
{
    public Address Location { get; init; } = null!;
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public DateTime StartDay { get; init; }
    public DateTime EndDay { get; init; }
    public AppointmentLimitation AppointmentLimitation { get; init; } = null!;

    public string OrganizerId { get; set; } = null!;
    public Account? Organizer { get; set; }
    public Chat? Chat { get; set; }
}