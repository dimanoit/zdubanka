using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class Account: IdentityUser
{
    public string FullName { get; init; } = null!;
    public string? Bio { get; init; }
    public string? ImageUrl { get; init; }
    public Gender? Gender { get; init; }
    public RelationshipStatus? RelationshipStatus { get; init; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public AuthMethod AuthMethod { get; set; }
    public ICollection<UserLanguage>? UserLanguages { get; init; }
    public ICollection<Appointment>? Appointments { get; set; }
    public ICollection<AppointmentParticipant>? AppointmentParticipations { get; set; }

}