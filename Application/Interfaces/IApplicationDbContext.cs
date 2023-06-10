using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Event> Events { get; }
    DbSet<Chat> Chats { get; }
    DbSet<Message> Messages { get; }
    DbSet<Account> Accounts { get; }
    DbSet<EventParticipant> EventParticipants { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}