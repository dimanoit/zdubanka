using Application.Interfaces;
using ApplicationUnitTests.Helpers;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ApplicationUnitTests.Fixtures;

public class TestDbContext :DbContext, IApplicationDbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Event> Events { get; set; } 
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<EventParticipant> EventParticipants { get; set; }
  

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountConfiguration).Assembly);
        
        modelBuilder.HasJsonConversion<Account, ICollection<UserLanguage>>(
            account => account.UserLanguages);
        
        modelBuilder.HasJsonConversion<Chat, string[]>(
            chat => chat.Members);

        modelBuilder.HasJsonConversion<Event, EventLimitation>(
            @event => @event.EventLimitation);
        
        modelBuilder.HasJsonConversion<Event, Address>(
            @event => @event.Location);
        base.OnModelCreating(modelBuilder);
    }
}