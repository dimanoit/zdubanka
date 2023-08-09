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
    private readonly string _sendGridSenderEmail;
    private readonly string _sendGridCompanyName;
    private static string _confirmationTemplateId;
    private static string _resetTemplateId;
    private readonly ILogger<EmailService> _logger;
    public EmailService(ILogger<EmailService> logger, IConfiguration configuration, ISendGridClient client)
    {
        _sendGridSenderEmail = configuration["SendGrid:SenderEmail"];
        _sendGridCompanyName = configuration["SendGrid:CompanyName"];
        _confirmationTemplateId = configuration["SendGrid:ConfirmationTemplateId"];
        _resetTemplateId = configuration["SendGrid:ResetTemplateId"];
        _logger = logger;
        _client = client;
    }
    public Dictionary<SendEmailBaseRequest, string> templateSelector = new Dictionary<SendEmailBaseRequest, string>
    {
        { new SendConfirmationEmailRequest(), _confirmationTemplateId },
        { new SendResetEmailRequest(), _resetTemplateId },
    };
    public async Task SendEmailAsync(SendEmailBaseRequest baseRequest, Dictionary<SendEmailBaseRequest, string> templateSelector)
    {
        if (!templateSelector.TryGetValue(baseRequest, out string templateId))
        {
            _logger.LogError($"Template not found for {baseRequest.GetType().Name}");
            return;
        }
        var dynamicData = new
        {
            confirmation_link = "test link 1",
            reset_link = "test link 2"
        };

        var from = new EmailAddress(_sendGridSenderEmail, _sendGridCompanyName);
        var to = new EmailAddress(baseRequest.RecipientEmail);
        var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, dynamicData);
        try
        {
            var response = await _client.SendEmailAsync(msg);
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                _logger.LogInformation($"Email sent to {baseRequest.RecipientEmail}");
            }
            else
            {
                _logger.LogError($"Failed to send email. Status Code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email:");
        }
    }



}

