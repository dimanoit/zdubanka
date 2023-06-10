namespace Domain.Requests.Chat;

public record SendMessageRequest
{
    public string SenderId { get; init; } = null!;
    public string Content { get; init; } = null!;
    public string ChatId { get; init; } = null!;
}