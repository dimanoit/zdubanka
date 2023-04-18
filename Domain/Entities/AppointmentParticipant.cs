using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class AppointmentParticipant : BaseEntity
{
    public string UserId { get; init; } = null!;
    public string AppointmentId { get; init; } = null!;
    public ParticipantStatus Status { get; init; } = ParticipantStatus.InReview;

    public Appointment Appointment { get; set; } = null!;
    public Account Account { get; set; } = null!;
}