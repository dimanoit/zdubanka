using Domain.Requests.Common;

namespace Application.Models.Requests.Events;

public record EventRetrieveRequest : PageRequestBase
{
    public string UserId { get; init; } = null!;
}