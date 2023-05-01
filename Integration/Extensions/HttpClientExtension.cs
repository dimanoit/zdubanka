using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Domain.Requests;

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
        T data) where T : class
    {
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        await SetAuthorizationHeader(client);
        return await client.PostAsync(url, content);
    }

    public static async Task<HttpResponseMessage> PatchAuth(
        this HttpClient client,
        string url)
    {
        await SetAuthorizationHeader(client);
        return await client.PatchAsync(url, null);
    }


    public static async Task<HttpResponseMessage> GetAuth(
        this HttpClient client,
        string url) 
    {
        await SetAuthorizationHeader(client);
        return await client.GetAsync(url);
    }


    #region Set up Token

    private static async Task SetAuthorizationHeader(HttpClient client)
    {
        var token = await GetToken(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private static readonly SemaphoreSlim TokenLock = new(1);

    private static async ValueTask<string> GetToken(HttpClient client)
    {
        if (!string.IsNullOrEmpty(SharedTestData.Token)) return SharedTestData.Token;

        await TokenLock.WaitAsync();
        try
        {
            var token = await GetTokenFromClient(client);
            SharedTestData.Token = token;
            return token;
        }
        finally
        {
            TokenLock.Release();
        }
    }

    private static async Task<string> GetTokenFromClient(HttpClient client)
    {
        var userSignInModel = new AuthenticationRequest
        {
            Email = SharedTestData.TestEmail,
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