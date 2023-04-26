using System.Net;

namespace Domain.Models;

public record RestErrorDetails(string Message, HttpStatusCode StatusCode = HttpStatusCode.InternalServerError);