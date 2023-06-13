namespace Domain.Requests.Chat;

public record DeleteChatRequest
{
    public string ChatId { get; init; } = null!;
}