using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions;

public static class HttpResponseExtensions
{
    public static ActionResult<T> ErrorResponse<T>(this RestErrorDetails? details)
    {
        return new BadRequestObjectResult(details.Message)
        {
            StatusCode = (int)details.StatusCode
        }!;
    }
}