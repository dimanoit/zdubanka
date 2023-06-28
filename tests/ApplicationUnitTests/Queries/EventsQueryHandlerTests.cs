using Application.Mappers;
using Application.Queries;
using ApplicationUnitTests.Fakers;
using ApplicationUnitTests.Helpers;
using Bogus;
using Bogus.DataSets;
using Domain.Entities;
using Domain.Enums;
using Domain.Requests;
using FluentAssertions;
using MediatR;
using Xunit;

namespace ApplicationUnitTests.Queries;

public class EventsQueryHandlerTests
{
    [Fact]
    public async Task Handle_WithValidRequest_ReturnsEventResponse()
    {
        // Arrange
        var query = new EventsQuery(GetSearchEventRequest());

        var dbContext = ApplicationDbContextFactory.Create();
        dbContext.Events.AddRange(EventFaker.CreateEvents(20));

        var handler = new EventsQueryHandler(dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
    }

    private static SearchEventRequest GetSearchEventRequest()
    {
        return new SearchEventRequest
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(7),
            SearchKeyword = "a",
            PeopleCount = 10,
            Gender = Gender.Female,
            MinAge = 10,
            MaxAge = 50,
            Skip = 1,
            Take = 100,
            RelationshipStatus = RelationshipStatus.Single,
            DistanceFromKm = 2000,
            Latitude = new Address().Latitude(),
            Longitude = new Address().Longitude()
        };
    }
}