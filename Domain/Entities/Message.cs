using Domain.Common;

namespace Domain.Entities;

public class Message : BaseEntity
{
    public string SenderId { get; init; } = null!;
    public string Content { get; init; } = null!;
    public DateTime SentDate { get; init; }

    public string ChatId { get; init; } = null!;
    public Chat Chat { get; init; } = null!;
}