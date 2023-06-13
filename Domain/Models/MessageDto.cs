namespace Domain.Models;

public record MessageDto
{
    public string SenderId { get; init; } = null!;
    public string Content { get; init; } = null!;
    public DateTime SentDate { get; init; }
    public string ChatId { get; init; } = null!;
    public string Id { get; init; } = null!;
}