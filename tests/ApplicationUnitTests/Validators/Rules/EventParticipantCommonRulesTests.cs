using Application.Interfaces;
using Application.Validation.Rules;
using ApplicationUnitTests.Fakers;
using ApplicationUnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace ApplicationUnitTests.Validators.Rules;

public class EventParticipantCommonRulesTests
{
    private readonly IApplicationDbContext _dbContext;

    public EventParticipantCommonRulesTests()
    {
        _dbContext = ApplicationDbContextFactory.Create();
    }

    [Fact]
    public async Task IsEventBelongsToOrganizerAsync_WithMatchingEventParticipant_ReturnsTrue()
    {
        // Arrange 
        var @event = EventFaker.CreateEventWithParticipants();
        _dbContext.Events.Add(@event);
        await _dbContext.SaveChangesAsync();

        // Act 
        var result = await _dbContext
            .IsEventBelongsToOrganizerAsync(@event.OrganizerId, @event.EventParticipants!.First().Id, default);


        // Assert
        result.Should().BeTrue();
    }
}