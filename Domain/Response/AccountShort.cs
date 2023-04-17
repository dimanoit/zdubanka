namespace Domain.Response;

public record AccountShort
{
    public string Email { get; init; } = null!;
    public string FullName { get; init; } = null!;
    public string ProfileImg { get; init; } = null!;
    public string Token { get; init; } = null!;
}