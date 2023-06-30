using Application.Queries;
using ApplicationUnitTests.Fixtures;
using ApplicationUnitTests.Helpers;
using Domain.Entities;
using Domain.Requests;
using FluentAssertions;
using Xunit;

namespace ApplicationUnitTests.Queries;

public class EventsQueryHandlerTests
{
    [Theory]
    [ClassData(typeof(EventsQueryFixture))]
    public async Task Handle_WithValidRequest_ReturnsEventResponse(Event @event, SearchEventRequest request)
    {
        // Arrange
        var query = new EventsQuery(request);

        var dbContext = ApplicationDbContextFactory.Create();
        dbContext.Events.Add(@event);
        await dbContext.SaveChangesAsync();
        var handler = new EventsQueryHandler(dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        @event.Id.Should().Be(result.Data.First().Id);
    }
}