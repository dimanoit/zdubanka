using Domain.Requests.Chat;
using Domain.Response;

namespace Application.Services.Interfaces;

public interface IMessageService
{
    Task SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken);
    Task DeleteMessagesAsync(DeleteMessageRequest request, CancellationToken cancellationToken);
    Task<GetMessagesResponse> GetMessagesAsync(GetMessagesRequest request, CancellationToken cancellationToken);
}