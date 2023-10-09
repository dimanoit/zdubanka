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
        var formData = new Dictionary<string, string>
        {
            { "Location.Street", eventRequest.Location.Street },
            { "Location.City", eventRequest.Location.City },
            { "Location.State", eventRequest.Location.State },
            { "Location.Country", eventRequest.Location.Country },
            { "Title", eventRequest.Title },
            { "Description", eventRequest.Description },
            { "StartDay", eventRequest.StartDay.ToString("yyyy-MM-ddTHH:mm:ss") },
            { "EndDay", eventRequest.EndDay.ToString("yyyy-MM-ddTHH:mm:ss") },
            { "Longitude", eventRequest.Longitude.ToString() },
            { "Latitude", eventRequest.Latitude.ToString() },
            { "EventLimitation.CountOfPeople", eventRequest.EventLimitation.CountOfPeople.ToString() },
            { "EventLimitation.Gender[0]", eventRequest.EventLimitation.Gender[0].ToString() },
            { "EventLimitation.Gender[1]", eventRequest.EventLimitation.Gender[1].ToString() },
            { "EventLimitation.RelationshipStatus[0]", eventRequest.EventLimitation.RelationshipStatus[0].ToString() },
            { "EventLimitation.AgeLimit.Min", eventRequest.EventLimitation.AgeLimit.Min.ToString() },
            { "EventLimitation.AgeLimit.Max", eventRequest.EventLimitation.AgeLimit.Max.ToString() }
        };

        var result = await client.PostAuth("api/event", new FormUrlEncodedContent(formData), SharedTestData.TestEmail);

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