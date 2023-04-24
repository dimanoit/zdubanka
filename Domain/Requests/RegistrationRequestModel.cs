namespace Domain.Requests;

public record RegistrationRequestModel
{
    public string Email { get; init; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
}