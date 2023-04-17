using Domain.Requests.Common;

namespace Domain.Requests;

public record AppointmentRetrieveRequest : PageRequestBase
{
    public string Email { get; init; }
}