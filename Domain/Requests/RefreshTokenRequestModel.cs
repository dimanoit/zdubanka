namespace Domain.Requests;

public record RefreshTokenRequestModel
{
    public string AccessToken { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
}