using Application.Mappers;
using Application.Queries;
using Bogus;
using Domain.Entities;
using Domain.Requests;
using MediatR;
using Xunit;

namespace ApplicationUnitTests.Queries;

public class EventsQueryHandlerTests
{
    private readonly IMediator _mediator;

    [Fact]
    public async Task Handle_WithValidRequest_ReturnsEventResponse()
    {
        // Arrange
        var query = new EventsQuery(new SearchEventRequest
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(7),
            SearchKeyword = "test",
            PeopleCount = 10
            // Add other properties for the request
        });

        var events = GenerateFakeEvents(10);
        var eventResponseDtos = events.Select(ev => ev.ToEventResponseDto()).ToList();
        var totalCount = eventResponseDtos.Count;

        _dbContext.Events.Returns(events);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().BeEquivalentTo(eventResponseDtos);
        result.TotalCount.Should().Be(totalCount);
    }

    // Add more test methods for different scenarios

    private List<Event> GenerateFakeEvents(int count)
    {
        var faker = new Faker<Event>()
            .RuleFor(ev => ev.Id, f => f.Random.Guid())
            .RuleFor(ev => ev.Title, f => f.Lorem.Sentence())
            .RuleFor(ev => ev.Description, f => f.Lorem.Paragraph())
            // Add other properties for the Event entity
            .Generate(count);

        return faker;
    }
}