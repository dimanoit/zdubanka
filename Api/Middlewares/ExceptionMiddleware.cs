using System.Text.Json;
using Api.Models;

namespace Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

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

            var handlingResponse = HandleException(ex);
            await WriteResponseAsync(context, handlingResponse);
        }
    }

    private RestErrorDetails HandleException(Exception ex)
    {
        _logger.LogError(ex.Message, ex);
        throw new NotImplementedException();
    }

    private Task WriteResponseAsync(HttpContext context, RestErrorDetails details)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)details.StatusCode;

        return context.Response.WriteAsync(JsonSerializer.Serialize(details));
    }
}