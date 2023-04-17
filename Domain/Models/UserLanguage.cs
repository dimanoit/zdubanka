using Domain.Enums;

namespace Domain.Models;

public record UserLanguage
{
    public Language Language { get; init; }
    public LanguageFluency Fluency { get; init; }
}