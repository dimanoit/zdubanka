using Domain.Enums;

namespace Domain.Models;

public record EventParticipantDto
{
    public string Id { get; init; } = null!;
    public string UserId { get; init; } = null!;
    public string UserName { get; init; } = null!;
    public string EventId { get; init; } = null!;
    public string EventTitle { get; init; } = null!;
    public ParticipantStatus Status { get; init; } = ParticipantStatus.InReview;
}