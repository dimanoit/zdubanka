using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Appointment> Appointments { get; }
    DbSet<Account> Accounts { get; }
    DbSet<Chat> Chats { get; }
    DbSet<Message> Messages { get; }
    DbSet<AppointmentParticipant> AppointmentParticipants { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}