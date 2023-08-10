namespace Domain.Models;

public record SendEmailBaseRequest
{
    public string RecipientEmail { get; set; } = null!;

    public string SenderEmail { get; set; } = null!;

}