using Domain.Enums;
using Domain.Requests.Common;

namespace Domain.Requests;

public record SearchEventRequest : PageRequestBase
{
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string? SearchKeyword { get; init; }
    public int? PeopleCount { get; init; }
    public Gender? Gender { get; init; }
    public RelationshipStatus? RelationshipStatus { get; init; }
    public int? MinAge { get; init; }
    public int? MaxAge { get; init; }
    public int? DistanceFromKm { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
}