using Application.EventHandlers;
using Application.Interfaces;
using ApplicationUnitTests.Fakers;
using ApplicationUnitTests.Fixtures;
using ApplicationUnitTests.Helpers;
using Domain.Entities;
using Domain.Enums;
using Domain.Events;
using FluentAssertions;
using Xunit;

namespace ApplicationUnitTests.Tests;

public class ParticipantAcceptedEventHandlerTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ParticipantAcceptedEventHandler _handler;

    public ParticipantAcceptedEventHandlerTests()
    {
        _dbContext = ApplicationDbContextFactory.Create();
        _handler = new ParticipantAcceptedEventHandler(_dbContext);
    }

    [Theory]
    [ClassData(typeof(EventParticipantsFixture))]
    public async Task Handle_ShouldCloseOrNotCloseEvent_DependsOnAlreadyAcceptedParticipants(
        Event eventEntity,
        EventStatus status)
    {
        // Arrange
        _dbContext.Events.Add(eventEntity);
        await _dbContext.SaveChangesAsync();

        var notification = new ParticipantAcceptedEvent(string.Empty, eventEntity.Id);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        eventEntity.Status.Should().Be(status);
    }
}