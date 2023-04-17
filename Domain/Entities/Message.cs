namespace Domain.Entities;

public class Message
{
    public string Id { get; init; } = null!;
    public string SenderId { get; init; } = null!;
    public string Content { get; set; } = null!;
    public DateTime SentDate { get; set; }

    public string ChatId { get; set; } = null!;
    public Chat Chat { get; set; }
}