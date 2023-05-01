using System.Net;
using System.Text.Json;
using Api;
using Domain.Enums;
using Domain.Models;
using Domain.Requests;
using FluentAssertions;
using Integration.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Integration.Controllers;

public class AppointmentControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AppointmentControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateAppointment_ShouldReturn_201StatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();
        var appointmentRequest = new AppointmentCreationRequest
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
            AppointmentLimitation = new AppointmentLimitation
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
        var result = await client.PostAuth("api/appointment", appointmentRequest);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task GetCurrentUserAppointments_ShouldReturn_AppointmentResultWithoutErrors()
    {
        // Arrange
        var client = _factory.CreateClient();
        var url = "api/appointment?skip=0&take=100";

        // Act
        var result = await client.GetAuth(url);

        // Assert
        result!.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonResult = await result.Content.ReadAsStringAsync();
        var totalCount = JsonDocument.Parse(jsonResult)
            .RootElement.GetProperty("totalCount").GetInt32();

        totalCount.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ApplyOnAppointmentAsync_ShouldReturn_HttpStatusCode200()
    {
        // Arrange
        var client = _factory.CreateClient();
        var response = await client.GetAuth("api/appointment?skip=0&take=100");
        var jsonResult = await response.Content.ReadAsStringAsync();
        var eventId = JsonDocument.Parse(jsonResult).RootElement.GetProperty("data")
            .EnumerateArray().Last()
            .GetProperty("id").GetString();
        
        // Act
        var result = await client.PatchAuth($"api/appointment/{eventId}/apply");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}