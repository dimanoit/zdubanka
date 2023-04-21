using Domain.Enums;

namespace Domain.Models;

public record EventParticipant
{
    public string UserId { get; init; } = null!;
    public string UserName { get; init; } = null!;
    public string AppointmentId { get; init; } = null!;
    public string AppointmentTitle { get; init; } = null!;
    public ParticipantStatus Status { get; init; } = ParticipantStatus.InReview;
}