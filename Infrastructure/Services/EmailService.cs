using System.Net;
using Application.Interfaces;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ISendGridClient _client;
    private readonly ILogger<EmailService> _logger;
    private readonly string _sendGridCompanyName;
    private readonly string _sendGridSenderEmail;

    private readonly Dictionary<Type, string?> _templateSelector;

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration, ISendGridClient client)
    {
        // TODO get it as a IOptions<>
        _sendGridSenderEmail = configuration["SendGrid:SenderEmail"];
        _sendGridCompanyName = configuration["SendGrid:CompanyName"];

        _templateSelector = new Dictionary<Type, string?>
        {
            { typeof(SendConfirmationEmailRequest), configuration["SendGrid:ConfirmationTemplateId"] },
            { typeof(SendResetEmailRequest), configuration["SendGrid:ResetTemplateId"] }
        };

        _logger = logger;
        _client = client;
    }

    public async Task SendEmailAsync(SendEmailBaseRequest baseRequest)
    {
        var templateId = GetTemplateId(baseRequest);

        var from = new EmailAddress(_sendGridSenderEmail, _sendGridCompanyName);
        var to = new EmailAddress(baseRequest.RecipientEmail);
        var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, baseRequest);
        try
        {
            var response = await _client.SendEmailAsync(msg);
            if (response.StatusCode == HttpStatusCode.Accepted)
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

    private string? GetTemplateId(SendEmailBaseRequest baseRequest)
    {
        if (_templateSelector.TryGetValue(baseRequest.GetType(), out var templateId))
        {
            return templateId;
        }

        _logger.LogError("Template not found for {baseRequest.GetType().Name}", baseRequest.GetType().Name);
        throw new NotFoundException();
    }
}