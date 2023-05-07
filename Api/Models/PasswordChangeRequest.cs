namespace Api.Models;

public record PasswordChangeRequest
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}