namespace Application.Services;

public interface IEmailService
{
    Task SendEmailAsync(string emailTo, string message, string subject);
}