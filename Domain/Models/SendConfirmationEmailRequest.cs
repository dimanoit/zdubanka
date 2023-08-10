namespace Domain.Models;

public record SendConfirmationEmailRequest : SendEmailBaseRequest
{
    public string ConfirmationLink { get; set; } = null;
}
