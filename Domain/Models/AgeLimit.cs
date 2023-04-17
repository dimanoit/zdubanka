namespace Domain.Models;

public record AgeLimit
{
    public int? Min { get; init; }
    public int? Max { get; init; }
}