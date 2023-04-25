namespace Domain.Response;

public record AuthenticationResponse
{
    public string Token { get; init; } = null!;
    public string? RefreshToken { get; init; } = null!;
    public DateTime Expiration { get; init; }
}