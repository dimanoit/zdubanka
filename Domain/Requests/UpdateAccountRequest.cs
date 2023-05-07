using Domain.Enums;
using Domain.Models;

namespace Domain.Requests;

public record UpdateAccountRequest
{
    public string UserId { get; init; } = null!;
    public string FullName { get; init; } = null!;
    public string? Bio { get; init; }
    public string? ImageUrl { get; init; }
    public RelationshipStatus? RelationshipStatus { get; init; }
    public ICollection<UserLanguage>? UserLanguages { get; init; }
}