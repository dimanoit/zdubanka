using Application.Interfaces;
using Application.Mappers;
using Application.Services.Interfaces;
using Domain.Requests.Chat;
using Domain.Response;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class MessageService : IMessageService
{
    private readonly IApplicationDbContext _dbContext;

    public MessageService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken)
    {
        var messageEntity = request.ToMessageEntity();
        _dbContext.Messages.Add(messageEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteMessagesAsync(DeleteMessageRequest request, CancellationToken cancellationToken)
    {
        var message = await _dbContext.Messages
            .FirstOrDefaultAsync(m => m.Id == request.MessageId, cancellationToken);

        if (message == null) return;

        _dbContext.Messages.Remove(message);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetMessagesResponse> GetMessagesAsync(
        GetMessagesRequest request,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Messages
            .AsNoTracking()
            .Where(m => m.ChatId == request.ChatId);

        var messages = await query
            .Skip(request.Skip)
            .Take(request.Take)
            .Select(m => m.ToMessageDto())
            .ToArrayAsync(cancellationToken);

        var totalCount = await query.CountAsync(cancellationToken);

        return new GetMessagesResponse
        {
            Data = messages,
            TotalCount = totalCount
        };
    }
}