using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class ApplicationDbContext : IdentityUserContext<Account>, IApplicationDbContext
{
    private readonly IMediator _mediator;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options
        , IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
        Database.Migrate();
    }

    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<EventParticipant> EventParticipants { get; set; } = null!;

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AccountConfiguration).Assembly);
        base.OnModelCreating(builder);
    }
}