using Domain.Models;

namespace Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(SendEmailRequest request ,string templateId,string link);
}