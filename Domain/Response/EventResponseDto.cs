using Domain.Enums;
using Domain.Models;

namespace Domain.Response;

public record EventResponseDto
{
    public string Id { get; init; } = null!;
    public Address Location { get; init; } = null!;
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string PictureUrl { get; init; } = null!;
    public DateTime StartDay { get; init; }
    public DateTime EndDay { get; init; }
    public EventStatus Status { get; init; }
    public EventLimitation EventLimitation { get; init; } = null!;
    public AccountShort Organizer { get; init; } = null!;
}