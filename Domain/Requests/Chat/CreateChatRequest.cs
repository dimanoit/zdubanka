namespace Domain.Requests.Chat;

public record CreateChatRequest
{
    public string Name { get; init; } = null!;
    public string[] Members { get; init; } = null!;
}