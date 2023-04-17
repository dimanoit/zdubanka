using Domain.Enums;
using Domain.Models;

namespace Domain.Entities;

public class Account
{
    public string Id { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string FullName { get; init; } = null!;
    public string? Bio { get; init; }
    public string? ImageUrl { get; init; }
    public Gender? Gender { get; init; }
    public RelationshipStatus? RelationshipStatus { get; init; }
    public ICollection<UserLanguage>? UserLanguages { get; init; }
    public ICollection<Appointment>? Appointments { get; set; }
    public AccountToken Token { get; set; }
}