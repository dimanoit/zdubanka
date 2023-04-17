using Domain.Entities;

namespace Application.Interfaces;

public interface IMessageService
{
    Task AddMessageAsync(Message message);
    Task<Message?> GetMessageByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<List<Message>> GetAllMessagesAsync();
    Task UpdateMessageAsync(Message message);
    Task DeleteMessageAsync(Message message);
}