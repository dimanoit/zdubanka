using Application.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string emailTo, string message, string subject)
    {
        await Task.CompletedTask;
        _logger.LogInformation("Mock email sent");
    }
}