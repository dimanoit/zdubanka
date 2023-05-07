using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Domain.Requests;
using Integration.Fixtures;

namespace Integration.Extensions;

public static class HttpClientExtension
{
    public static async Task<HttpResponseMessage> Post<T>(
        this HttpClient client,
        string url,
        T data) where T : class
    {
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        return await client.PostAsync(url, content);
    }

    public static async Task<HttpResponseMessage> PostAuth<T>(
        this HttpClient client,
        string url,
        T data, string email) where T : class
    {
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        await SetAuthorizationHeader(client, email);
        return await client.PostAsync(url, content);
    }

    public static async Task<HttpResponseMessage> PutAuth<T>(
        this HttpClient client,
        string url,
        T data, string email) where T : class
    {
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        await SetAuthorizationHeader(client, email);
        return await client.PutAsync(url, content);
    }

    public static async Task<HttpResponseMessage> PatchAuth(
        this HttpClient client,
        string url,
        string email)
    {
        await SetAuthorizationHeader(client, email);
        return await client.PatchAsync(url, null);
    }


    public static async Task<HttpResponseMessage> GetAuth(
        this HttpClient client,
        string url,
        string email)
    {
        await SetAuthorizationHeader(client, email);
        return await client.GetAsync(url);
    }



    #region Set up Token

    private static async Task SetAuthorizationHeader(HttpClient client, string email)
    {
        var token = await GetToken(client, email);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private static readonly SemaphoreSlim TokenLock = new(1);

    private static async ValueTask<string> GetToken(HttpClient client, string email)
    {
        if (email == SharedTestData.TestEmail && !string.IsNullOrEmpty(SharedTestData.Token))
            return SharedTestData.Token;

        if (email == SharedTestData.TestEmailSecondUser && !string.IsNullOrEmpty(SharedTestData.TokenSecondUser))
            return SharedTestData.TokenSecondUser;

        await TokenLock.WaitAsync();
        try
        {
            var token = await GetTokenFromClient(client, email);
            if (email == SharedTestData.TestEmail) SharedTestData.Token = token;
            if (email == SharedTestData.TestEmailSecondUser) SharedTestData.TokenSecondUser = token;
            return token;
        }
        finally
        {
            TokenLock.Release();
        }
    }

    private static async Task<string> GetTokenFromClient(HttpClient client, string email)
    {
        var userSignInModel = new AuthenticationRequest
        {
            Email = email,
            Password = "somePassword123"
        };

        var response = await client.Post("api/auth/token", userSignInModel);

        var responseText = await response.Content.ReadAsStringAsync();

        var jsonDoc = JsonDocument.Parse(responseText);
        var token = jsonDoc.RootElement.GetProperty("token").GetString();
        return token;
    }

    #endregion
}