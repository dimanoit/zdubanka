using Domain.Enums;

namespace Domain.Requests;

public record RegistrationRequestModel
{
    public string Email { get; init; } = null!;
    public string UserName { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string Name { get; init; } = null!;
    public Gender Gender { get; init; }
    public DateTime DateOfBirth { get; init; }
}