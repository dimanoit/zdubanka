namespace Domain.Models;

public record ChatDto
{
    public string Id { get; init; } = null!;
    public string[] Members { get; init; } = null!;
    public string Name { get; init; } = null!;
}