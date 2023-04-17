namespace Application.Services;

public interface IEmailService
{
    Task SendEmail(string emailTo, string message, string subject);
}