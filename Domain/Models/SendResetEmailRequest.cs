namespace Domain.Models;

public record SendResetEmailRequest : SendEmailBaseRequest
{
    public string TemplateId { get; set; } = null;
    public string ResetLink { get; set; } = null;
}