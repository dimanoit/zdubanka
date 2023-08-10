namespace Infrastructure;

public class SendGridSettings
{
    public string ApiKey { get; set; }
    public string SenderEmail { get; set; }
    public string CompanyName { get; set; }
    public string ConfirmationTemplateId { get; set; }
    public string ResetTemplateId { get; set; }
}