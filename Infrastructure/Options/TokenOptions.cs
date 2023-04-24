namespace Infrastructure.Options;

public record TokenOptions
{
    public string Key { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string Subject { get; init; } = null!;
    public int ExpirationMinutes { get; init; } = 60;
}