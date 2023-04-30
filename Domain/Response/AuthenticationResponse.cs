namespace Domain.Response;

public record AuthenticationResponse
{
    public string Token { get; init; } = null!;
    public string? RefreshToken { get; init; }
    public DateTime Expiration { get; init; }
}