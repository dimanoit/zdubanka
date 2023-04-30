using Domain.Enums;

namespace Domain.Models;

public record AppointmentLimitation
{
    public int CountOfPeople { get; init; } = 2;
    public Gender[] Gender { get; init; } = Array.Empty<Gender>();
    public RelationshipStatus[] RelationshipStatus { get; init; } = Array.Empty<RelationshipStatus>();
    public AgeLimit AgeLimit { get; init; }
}