using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class ApplicationDbContext :  DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options
       ,IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
        Database.Migrate();
    }

    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<AppointmentParticipant> AppointmentParticipants { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}