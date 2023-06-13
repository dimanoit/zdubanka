using Domain.Models;
using Domain.Requests.Chat;
using Domain.Response;

namespace Application.Services.Interfaces;

public interface IChatService
{
    Task CreateAsync(CreateChatRequest request, CancellationToken cancellationToken);
    Task DeleteAsync(DeleteChatRequest request, CancellationToken cancellationToken);
    Task UpdateAsync(UpdateChatRequest request, CancellationToken cancellationToken);
    Task<ChatDto?> GetAsync(string id, CancellationToken cancellationToken);
    Task<GetChatResponse> GetAsync(GetChatRequest request, CancellationToken cancellationToken);
}