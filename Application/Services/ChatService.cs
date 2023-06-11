using Application.Interfaces;
using Application.Mappers;
using Application.Services.Interfaces;
using Domain.Models;
using Domain.Requests.Chat;
using Domain.Response;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ChatService : IChatService
{
    private readonly IValidator<CreateChatRequest> _createChatRequestValidator;
    private readonly IApplicationDbContext _dbContext;
    private readonly IValidator<DeleteChatRequest> _deleteChatRequestValidator;
    private readonly IValidator<UpdateChatRequest> _updateChatRequestValidator;

    public ChatService(
        IApplicationDbContext dbContext,
        IValidator<CreateChatRequest> createChatRequestValidator,
        IValidator<UpdateChatRequest> updateChatRequestValidator,
        IValidator<DeleteChatRequest> deleteChatRequestValidator)
    {
        _dbContext = dbContext;
        _createChatRequestValidator = createChatRequestValidator;
        _updateChatRequestValidator = updateChatRequestValidator;
        _deleteChatRequestValidator = deleteChatRequestValidator;
    }

    public async Task CreateAsync(CreateChatRequest request, CancellationToken cancellationToken)
    {
        await _createChatRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var chat = request.ToChatEntity();
        _dbContext.Chats.Add(chat);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(DeleteChatRequest request, CancellationToken cancellationToken)
    {
        await _deleteChatRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var chat = await _dbContext.Chats
            .FirstOrDefaultAsync(c => c.Id == request.ChatId, cancellationToken);

        if (chat == null) return;

        _dbContext.Chats.Remove(chat);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UpdateChatRequest request, CancellationToken cancellationToken)
    {
        await _updateChatRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

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