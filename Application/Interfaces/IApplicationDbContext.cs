using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Appointment> Appointments { get; set; }
    DbSet<Account> Accounts { get; set; }
    DbSet<Chat> Chats { get; set; }
    DbSet<Message> Messages { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}