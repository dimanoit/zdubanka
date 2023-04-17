using Domain.Requests.Common;

namespace Domain.Requests;

public record AppointmentRetrieveRequest : PageRequestBase
{
    public string UserId { get; init; }
}