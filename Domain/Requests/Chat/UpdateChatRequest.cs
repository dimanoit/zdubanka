namespace Domain.Requests.Chat;

public record UpdateChatRequest
{
    public string ChatId { get; init; } = null!;
    public string[] Members { get; init; } = Array.Empty<string>();
    public string Name { get; init; } = null!;
}