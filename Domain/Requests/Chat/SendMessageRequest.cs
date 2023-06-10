namespace Domain.Requests.Chat;

public record SendMessageRequest
{
    public string? Id { get; init; }
    public string SenderId { get; init; } = null!;
    public string Content { get; init; } = null!;
    public DateTime SentDate { get; init; }
    public string ChatId { get; init; } = null!;
}