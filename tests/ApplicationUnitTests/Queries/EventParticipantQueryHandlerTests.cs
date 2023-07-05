using Application.Queries;
using ApplicationUnitTests.Fakers;
using ApplicationUnitTests.Helpers;
using Domain.Requests;
using FluentAssertions;
using Xunit;

namespace ApplicationUnitTests.Queries;

public class EventParticipantQueryHandlerTests
{
    [Fact]
    public async Task Handle_WithValidRequest_ReturnsEventsParticipantsResponse()
    {
        // Arrange
        var eventWithParticipants = EventFaker.CreateEventWithParticipants();
        var request = new EventParticipantRequest(eventWithParticipants.Id, eventWithParticipants.OrganizerId);
        var query = new EventParticipantQuery(request);
        var dbContext = ApplicationDbContextFactory.Create();
        dbContext.Events.Add(eventWithParticipants);
        await dbContext.SaveChangesAsync();
        
        var handler = new EventParticipantQueryHandler(dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        eventWithParticipants.EventParticipants!.First().Id.Should().Be(result.Data.First().Id);
    }
}