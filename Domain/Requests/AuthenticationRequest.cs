namespace Domain.Requests;

public record AuthenticationRequest
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}