using System.Net;
using System.Text.Json;
using Api;
using FluentAssertions;
using Integration.Extensions;
using Integration.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Integration.Controllers;

public class EventParticipantControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public EventParticipantControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetEventParticipantsAsync_ShouldReturn_StatusCode200()
    {
        // Arrange
        var client = _factory.CreateClient();
        var url = "api/event-participants?eventId=";

        var response = await client.GetAuth("api/appointment?skip=0&take=100", SharedTestData.TestEmail);
        var jsonEventResult = await response.Content.ReadAsStringAsync();
        var eventId = JsonDocument.Parse(jsonEventResult).RootElement.GetProperty("data")
            .EnumerateArray().Last()
            .GetProperty("id").GetString();

        url += eventId;
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
    public async Task AcceptEventParticipantAsync_ShouldReturn_HttpStatusCode200()
    {
        // Arrange
        var client = _factory.CreateClient();
        var url = "api/event-participants?eventId=";

        var response = await client.GetAuth("api/appointment?skip=0&take=100", SharedTestData.TestEmail);
        var jsonEventResult = await response.Content.ReadAsStringAsync();
        var eventId = JsonDocument.Parse(jsonEventResult).RootElement.GetProperty("data")
            .EnumerateArray().Last()
            .GetProperty("id").GetString();

        url += eventId;
        var eventParticipantHttpResult = await client.GetAuth(url, SharedTestData.TestEmail);

        var jsonResult = await eventParticipantHttpResult.Content.ReadAsStringAsync();
        var eventParticipantId = JsonDocument.Parse(jsonResult).RootElement.GetProperty("data")
            .EnumerateArray().Last()
            .GetProperty("id").GetString();

        // Act
        var result = await client.PatchAuth(
            $"api/event-participants/{eventParticipantId}/accept",
            SharedTestData.TestEmail);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RejectEventParticipantAsync_ShouldReturn_HttpStatusCode200()
    {
        // Arrange
        var client = _factory.CreateClient();
        var url = "api/event-participants?eventId=";

        var response = await client.GetAuth("api/appointment?skip=0&take=100", SharedTestData.TestEmail);
        var jsonEventResult = await response.Content.ReadAsStringAsync();
        var eventId = JsonDocument.Parse(jsonEventResult).RootElement.GetProperty("data")
            .EnumerateArray().Last()
            .GetProperty("id").GetString();

        url += eventId;
        var eventParticipantHttpResult = await client.GetAuth(url, SharedTestData.TestEmail);

        var jsonResult = await eventParticipantHttpResult.Content.ReadAsStringAsync();
        var eventParticipantId = JsonDocument.Parse(jsonResult).RootElement.GetProperty("data")
            .EnumerateArray().Last()
            .GetProperty("id").GetString();

        // Act
        var result = await client.PatchAuth(
            $"api/event-participants/{eventParticipantId}/reject",
            SharedTestData.TestEmail);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}