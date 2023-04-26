using System.Text.Json;
using Api.Extensions;
using Domain.Models;

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

            _logger.LogError("Internal Server Error", ex);
            var response = new RestErrorDetails("Internal Server Error"); 
            
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsync(response.Message);
        }
    }
}