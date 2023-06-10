using Application.Interfaces;
using Application.Mappers;
using Application.Services.Interfaces;
using Domain.Models;
using Domain.Requests.Chat;
using Domain.Response;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ChatService : IChatService
{
    private readonly IApplicationDbContext _dbContext;

    public ChatService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(CreateChatRequest request, CancellationToken cancellationToken)
    {
        var chat = request.ToChatEntity();
        _dbContext.Chats.Add(chat);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(DeleteChatRequest request, CancellationToken cancellationToken)
    {
        var chat = await _dbContext.Chats
            .FirstOrDefaultAsync(c => c.Id == request.ChatId, cancellationToken);

        if (chat == null) return;

        _dbContext.Chats.Remove(chat);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UpdateChatRequest request, CancellationToken cancellationToken)
    {
        var chat = request.ToChatEntity();
        _dbContext.Chats.Update(chat);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ChatDto?> GetAsync(string id, CancellationToken cancellationToken)
    {
        return await _dbContext.Chats.AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => c.ToChatDto())
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<GetChatResponse> GetAsync(GetChatRequest request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Chats.AsNoTracking();

        var data = await query
            .Select(c => c.ToChatDto())
            .Skip(request.Skip)
            .Take(request.Take)
            .ToArrayAsync(cancellationToken);

        var totalCount = await query.CountAsync(cancellationToken);

        return new GetChatResponse
        {
            Data = data,
            TotalCount = totalCount
        };
    }
}