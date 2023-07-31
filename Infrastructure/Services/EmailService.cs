using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly string _sendGridApiKey;
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger,IConfiguration configuration)
    {
        _sendGridApiKey = configuration["SendGrid:ApiKey"];
        _logger = logger;
    }

    public async Task SendEmailAsync(SendEmailRequest request)
    {
        var client = new SendGridClient(_sendGridApiKey);
        var from = new EmailAddress("oleksiibahmet@gmail.com", "Zdubanka Inc.");
        var to = new EmailAddress(request.RecipientEmail);
        var msg = MailHelper.CreateSingleEmail(from, to, request.Subject, request.Message, null);
        //msg.SetTemplateId(request.TemplateId);
        
        try
        {
            var response = await client.SendEmailAsync(msg);
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                _logger.LogInformation($"Email sent to {request.RecipientEmail}");
            }
            else
            {
                _logger.LogError($"Failed to send email. Status Code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending email: {ex.Message}");
            // Handle the exception as required
        }
    }


   
}

