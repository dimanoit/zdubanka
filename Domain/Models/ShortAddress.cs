namespace Domain.Models;

public record ShortAddress
{
    public string City { get; init; } = null!;
    public string? State { get; init; }
    public string Country { get; init; } = null!;
}