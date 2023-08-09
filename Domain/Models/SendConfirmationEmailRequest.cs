namespace Domain.Models;

public record SendConfirmationEmailRequest : SendEmailBaseRequest
{
    public string TemplateId { get; set; } = null;
    public string ConfirmationLink { get; set; } = null;
}
