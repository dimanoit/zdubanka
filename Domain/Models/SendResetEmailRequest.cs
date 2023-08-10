namespace Domain.Models;

public record SendResetEmailRequest : SendEmailBaseRequest
{
    public string ResetLink { get; set; } = null;
}