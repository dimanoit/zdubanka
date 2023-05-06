using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class Account : IdentityUser
{
    public string FullName { get; set; } = null!;
    public string? Bio { get; set; }
    public string? ImageUrl { get; set; }
    public Gender? Gender { get; init; }
    public RelationshipStatus? RelationshipStatus { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public DateTime DateOfBirth { get; set; }
    public AuthMethod AuthMethod { get; set; }
    public ICollection<UserLanguage>? UserLanguages { get; set; }
    public ICollection<Appointment>? Appointments { get; set; }
    public ICollection<AppointmentParticipant>? AppointmentParticipations { get; set; }
}