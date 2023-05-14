using Domain.Requests.Common;

namespace Domain.Requests;

public record EventRetrieveRequest : PageRequestBase
{
    public string UserId { get; init; } = null!;
}