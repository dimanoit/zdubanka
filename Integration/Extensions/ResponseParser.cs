using System.Text.Json;
using System.Text.Json.Serialization;

namespace Integration.Extensions;

public static class ResponseParser
{
    public static async Task<T?> ParseJson<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode) throw new Exception("Received a error response from API");

        var responseJson = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        options.Converters.Add(new JsonStringEnumConverter());

        return JsonSerializer.Deserialize<T>(responseJson, options);
    }
}