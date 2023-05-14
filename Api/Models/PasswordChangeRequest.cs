namespace Api.Models;

public record PasswordChangeRequest
{
    public string Email { get; init; } = null!;
    public string Token { get; init; } = null!;
    public string NewPassword { get; init; } = null!;
}