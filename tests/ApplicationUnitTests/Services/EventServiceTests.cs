using Application.Interfaces;
using Application.Models.Requests.Events;
using Application.Services;
using ApplicationUnitTests.Fakers;
using ApplicationUnitTests.Helpers;
using Domain.Entities;
using Domain.Enums;
using Domain.Events;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace ApplicationUnitTests.Services;

public class EventServiceTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly EventService _eventService;

    public EventServiceTests()
    {
        _dbContext = ApplicationDbContextFactory.Create();
        _eventService = new EventService(_dbContext, Substitute.For<IFileService>());
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
    public async Task ApplyOnEventAsync_ValidParameters_AppliesParticipant()
    {
        // Arrange
        var account = AccountFaker.Create();
        _dbContext.Accounts.Add(account);
        var @event = await GetEventWithinDatabase();

        var eventService = new EventService(_dbContext, Substitute.For<IFileService>());

        // Act
        await eventService.ApplyOnEventAsync(@event.Id, account.Id, default);

        // Assert
        var eventParticipant = await _dbContext.EventParticipants
            .FirstAsync(ep => ep.UserId == account.Id && ep.EventId == @event.Id);

        eventParticipant.Status.Should().Be(ParticipantStatus.InReview);
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