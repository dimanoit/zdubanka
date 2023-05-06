using System.Net;
using Api;
using Domain.Entities;
using Domain.Requests;
using FluentAssertions;
using Integration.Extensions;
using Integration.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Integration.Controllers;

public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AccountControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAccountAsync_ShouldReturnAccount_StatusCode200()
    {
        // Arrange
        var client = _factory.CreateClient();
        var result = await client.GetAuth("api/account", SharedTestData.TestEmail);

        var data = await ResponseParser.ParseJson<Account>(result);

        // Assert
        data!.FullName.Should().NotBeNullOrEmpty();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateAccountAsync_ShouldReturnUpdatedAccount_StatusCode200()
    {
        // Arrange
        var client = _factory.CreateClient();
        var result = await client.GetAuth("api/account", SharedTestData.TestEmail);

        var data = await ResponseParser.ParseJson<Account>(result);
        string newFullName = data.FullName[..^1] +
                             (char)new Random().Next('a', 'z' + 1 - (data.FullName.EndsWith("a") ? 1 : 0));

        var updateAccountRequest = new UpdateAccountRequest()
        {
            Bio = data.Bio,
            FullName = newFullName,
            ImageUrl = data.ImageUrl,
            RelationshipStatus = data.RelationshipStatus,
            UserId = data.Id,
            UserLanguages = data.UserLanguages
        };

        //Act
        var response = await client.PutAuth("api/account", updateAccountRequest, SharedTestData.TestEmail);
        
        //Arrange
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result2 = await client.GetAuth("api/account", SharedTestData.TestEmail);
        var data2 = await ResponseParser.ParseJson<Account>(result2);
        data2.FullName.Should().Be(newFullName);
    }
}