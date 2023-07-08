using Application.Interfaces;
using Application.Services;
using ApplicationUnitTests.Fakers;
using ApplicationUnitTests.Helpers;
using Domain.Entities;
using Domain.Events;
using Domain.Requests;
using FluentAssertions;
using Xunit;

namespace ApplicationUnitTests.Services;

public class EventServiceTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly EventService _eventService;

    public EventServiceTests()
    {
        _dbContext = ApplicationDbContextFactory.Create();
        _eventService = new EventService(_dbContext);
    }

    [Fact]
    public async Task CreateAsync_ValidEventRequest_ReturnsCreatedEvent()
    {
        // Arrange
        var eventRequest = EventFaker.CreateEventCreationRequest();
        var organizerId = "organizerId";
        var cancellationToken = CancellationToken.None;

        // Act
        var createdEvent = await _eventService.CreateAsync(eventRequest, organizerId, cancellationToken);

        // Assert
        createdEvent.Should().NotBeNull();
        createdEvent.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<EventCreatedEvent>();
    }

        
    [Fact]
    public async Task GetUsersEventsAsync_ValidRequest_ReturnsEventResponse()
    {
        // Arrange
        var @event = await GetEventWithinDatabase();

        var request = new EventRetrieveRequest { UserId = @event.OrganizerId };

        // Act
        var eventResponse = await _eventService.GetUsersEventsAsync(request, default);

        // Assert
        eventResponse.Data.First().Id.Should().Be(@event.Id);
        eventResponse.TotalCount.Should().Be(1);
    }

    private async Task<Event> GetEventWithinDatabase()
    {
        var @event = EventFaker.CreateEvent();
        _dbContext.Events.Add(@event);
        await _dbContext.SaveChangesAsync();
        return @event;
    }
}