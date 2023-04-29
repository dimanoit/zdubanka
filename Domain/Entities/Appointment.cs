using Domain.Common;
using Domain.Enums;
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

    public EventStatus Status { get; set; }
    public string OrganizerId { get; set; } = null!;
    public Account? Organizer { get; set; }
    public Chat? Chat { get; set; }
    public ICollection<AppointmentParticipant>? AppointmentParticipants { get; set; }
}