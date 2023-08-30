using Application.Mappers;
using ApplicationUnitTests.Fakers;
using FluentAssertions;
using Xunit;

namespace ApplicationUnitTests.Mappers;

public class EventMappersTests
{
    [Fact]
    public void ToEvent_ShouldMapEventCreationRequestToEvent()
    {
        // Arrange
        var request = EventFaker.CreateEventCreationRequest();
        var organizerId = "TestOrganizerId";
        var pictureUrl = "some_url";

        // Act
        var result = request.ToEvent(organizerId, pictureUrl);

        // Assert
        result.Should().NotBeNull();
        result.Location.Should().Be(request.Location);
        result.Title.Should().Be(request.Title);
        result.Description.Should().Be(request.Description);
        result.StartDay.Should().Be(request.StartDay);
        result.EndDay.Should().Be(request.EndDay);
        result.EventLimitation.Should().Be(request.EventLimitation);
        result.OrganizerId.Should().Be(organizerId);
        result.Latitude.Should().Be(request.Latitude);
        result.Longitude.Should().Be(request.Longitude);
    }

    [Fact]
    public void ToEventResponseDto_ShouldMapEventToEventResponseDto()
    {
        // Arrange
        var entity = EventFaker.CreateEvent();

        // Act
        var result = entity.ToEventResponseDto();

        // Assert
        result.Should().NotBeNull();
        result.Location.Should().Be(entity.Location);
        result.Title.Should().Be(entity.Title);
        result.Description.Should().Be(entity.Description);
        result.StartDay.Should().Be(entity.StartDay);
        result.EndDay.Should().Be(entity.EndDay);
        result.EventLimitation.Should().Be(entity.EventLimitation);
        result.Id.Should().Be(entity.Id);
        result.Status.Should().Be(entity.Status);
    }
}