using System.Net;
using Api;
using Domain.Enums;
using Domain.Requests;
using FluentAssertions;
using Integration.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Integration.Controllers;

public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task RegisterNewAccount_ShouldReturn_201StatusCode_OR_Already_Exist_Account()
    {
        var client = _factory.CreateClient();

        var userRegistrationModel = new RegistrationRequestModel()
        {
            Email = SharedTestData.TestEmail,
            Name = "Dimonchik Testyvalbnuk",
            Password = "somePassword123",
            DateOfBirth = DateTime.UtcNow.AddYears(-20),
            UserName = "Dimonchik",
            Gender = Gender.Male
        };

        var response = await client.Post("api/auth", userRegistrationModel);

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var responseText = await response.Content.ReadAsStringAsync();
            responseText.Should().Contain("DuplicateEmail");
            return;
        }

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task SignInTestAccount_ShouldReturn_200StatusCode()
    {
        var client = _factory.CreateClient();
        var userSignInModel = new AuthenticationRequest()
        {
            Email = SharedTestData.TestEmail,
            Password = "somePassword123",
        };

        var response = await client.Post("api/auth/token", userSignInModel);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}