using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ISendGridClient _client;
    private readonly string _sendGridApiKey;
    private readonly string _sendGridSenderEmail;
    private readonly string _sendGridCompanyName;
    private readonly ILogger<EmailService> _logger;
    private readonly string _templateId;

    public EmailService(ILogger<EmailService> logger,IConfiguration configuration,ISendGridClient client)
    {
        _sendGridApiKey = configuration["SendGrid:ApiKey"];
        _sendGridSenderEmail = configuration["SendGrid:SenderEmail"];
        _sendGridCompanyName = configuration["SendGrid:CompanyName"];
        _templateId = configuration["SendGrid:TemplateId"];
        _logger = logger;
        _client = client;
    }

    public async Task SendEmailAsync(SendEmailRequest request,string templateId,string confirmationLink)
    {
        var dynamicData = new
        {
            confirmation_link = confirmationLink
        };
        var from = new EmailAddress(_sendGridSenderEmail,_sendGridCompanyName);
        var to = new EmailAddress(request.RecipientEmail);
        var msg = MailHelper.CreateSingleTemplateEmail(from, to,templateId, dynamicData);
        try
        {
            var response = await _client.SendEmailAsync(msg);
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
            _logger.LogError(ex,"Error sending email:");
            // Handle the exception as required
        }
    }


   
}

