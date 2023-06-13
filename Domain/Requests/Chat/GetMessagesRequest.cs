using Domain.Requests.Common;

namespace Domain.Requests.Chat;

public record GetMessagesRequest : PageRequestBase
{
    public string ChatId { get; init; } = null!;
}