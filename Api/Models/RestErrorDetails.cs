using System.Net;

namespace Api.Models;

public record RestErrorDetails
{
    public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.InternalServerError;
    public string Message { get; init; } = null!;
}