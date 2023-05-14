using System.Net.Mime;
using Domain.Models;

namespace Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted)
            {
                throw;
            }

            _logger.LogError(ex, "Internal Server Error");
            var response = new RestErrorDetails("Internal Server Error");

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsync(response.Message);
        }
    }
}