namespace Domain.Requests;

public record AuthenticationRequest
{
    public string UserName { get; init; } = null!;
    public string Password { get; init; } = null!;
}