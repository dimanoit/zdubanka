namespace Domain.Models;

public record SendEmailRequest
{
    public string RecipientEmail { get; set; } = null!;
    
    public string SenderEmail { get; set; }= null!;
    
    public string Message { get; set; }= null!;
    
    public string Subject { get; set; }= null!;
    
   // public string TemplateId { get; set; }= null!;
    
}