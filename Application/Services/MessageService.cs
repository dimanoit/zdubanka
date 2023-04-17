using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class MessageService : IMessageService
{
    private readonly IApplicationDbContext _context;

    public MessageService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddMessageAsync(Message message)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
    }

    public async Task<Message?> GetMessageByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<List<Message>> GetAllMessagesAsync()
    {
        return await _context.Messages.ToListAsync();
    }

    public async Task UpdateMessageAsync(Message message)
    {
        _context.Messages.Update(message);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMessageAsync(Message message)
    {
        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();
    }
}