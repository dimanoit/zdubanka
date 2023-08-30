using System.Net;
using System.Text.Json;
using Api;
using Application.Models.Requests.Events;
using Domain.Enums;
using Domain.Models;
using FluentAssertions;
using Integration.Extensions;
using Integration.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Integration.Controllers;

public class EventControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public EventControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateEvent_ShouldReturn_201StatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();
        var eventRequest = new EventCreationRequest
        {
            Location = new Address
            {
                Street = "123 Main St",
                City = "Anytown",
                State = "CA",
                Country = "USA"
            },
            Title = "Important Meeting",
            Description = "Discuss project progress",
            StartDay = DateTime.UtcNow.AddDays(1),
            EndDay = DateTime.UtcNow.AddDays(1).AddHours(2),
            Longitude = CoordinatesFixture.GenerateLongitude(),
            Latitude = CoordinatesFixture.GenerateLatitude(),
            EventLimitation = new EventLimitation
            {
                CountOfPeople = 3,
                Gender = new[] { Gender.Male, Gender.Female },
                RelationshipStatus = new[] { RelationshipStatus.Single },
                AgeLimit = new AgeLimit
                {
                    Min = 18,
                    Max = 65
                }
            }
        };

        // Act 
        var result = await client.PostAuth("api/event", eventRequest, SharedTestData.TestEmail);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task GetCurrentUserEvents_ShouldReturn_EventResultWithoutErrors()
    {
        // Arrange
        var client = _factory.CreateClient();
        var url = "api/event/own?skip=0&take=100";

        // Act
        var result = await client.GetAuth(url, SharedTestData.TestEmail);

        // Assert
        result!.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonResult = await result.Content.ReadAsStringAsync();
        var totalCount = JsonDocument.Parse(jsonResult)
            .RootElement.GetProperty("totalCount").GetInt32();

        totalCount.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ApplyOnEventAsync_ShouldReturn_HttpStatusCode200()
    {
        // Arrange
        var client = _factory.CreateClient();
        var response = await client.GetAuth("api/event/own?skip=0&take=100", SharedTestData.TestEmail);
        var jsonResult = await response.Content.ReadAsStringAsync();
        var eventId = JsonDocument.Parse(jsonResult).RootElement.GetProperty("data")
            .EnumerateArray().Last()
            .GetProperty("id").GetString();

        // Act
        var result = await client.PatchAuth($"api/event/{eventId}/apply", SharedTestData.TestEmailSecondUser);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}