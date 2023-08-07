namespace Domain.Models;

public record SendEmailRequest
{
    public string RecipientEmail { get; set; } = null!;
    
    public string SenderEmail { get; set; }= null!;
    
}