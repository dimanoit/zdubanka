namespace Domain.Requests.Chat;

public record DeleteMessageRequest
{
    public string MessageId { get; init; } = null!;
}