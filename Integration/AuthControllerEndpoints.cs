using System.Net.Http.Json;
using System.Text;
using Api;
using Domain.Requests;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Integration;

public class AuthControllerEndpoints: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthControllerEndpoints(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task RegisterSignInNewAccount_ShouldReturn_200StatusCode()
    {
        var client = _factory.CreateClient();

        var userRegistrationModel = new RegistrationRequestModel()
        {
            Email = "TestEmail@gmail.com",
            Name = "Dimonchik Testyvalbnuk",
            Password = "Xexxeex XYI!"
        };

        var content = new StringContent(userRegistrationModel.ToString(), Encoding.UTF8, "application/json");
        var response = client.PostAsync("api/auth",jsonContent);

    }
}