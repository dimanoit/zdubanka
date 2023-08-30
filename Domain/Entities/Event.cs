using Domain.Common;
using Domain.Enums;
using Domain.Models;

namespace Domain.Entities;

public class Event : BaseEntity
{
    public Address Location { get; init; } = null!;
    public string PictureUrl { get; init; } = null!;
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public DateTime StartDay { get; init; }
    public DateTime EndDay { get; init; }
    public EventLimitation EventLimitation { get; init; } = null!;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public EventStatus Status { get; set; } = EventStatus.Opened;
    public string OrganizerId { get; set; } = null!;
    public Account? Organizer { get; set; }
    public ICollection<EventParticipant>? EventParticipants { get; set; }
}