using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Models.Requests.Events;

public record EventCreationRequest
{
    public Address Location { get; init; } = null!;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public DateTime StartDay { get; init; }
    public DateTime EndDay { get; init; }
    public EventLimitation EventLimitation { get; init; } = null!;
    public IFormFile Picture { get; init; } = null!;
}