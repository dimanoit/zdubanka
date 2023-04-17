using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Api.Filters;
public class SocialAuthFilter : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("X-Google-Token", out var googleToken))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        using var scope = context.HttpContext.RequestServices.CreateScope();
        var applicationSettings = scope.ServiceProvider.GetRequiredService<IOptions<AppSettings>>().Value;

        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new List<string> { applicationSettings.GoogleClientId }
        };

        try
        {
            await GoogleJsonWebSignature.ValidateAsync(googleToken, settings);
        }
        catch (Exception e) // TODO check if we can catch more specific exception 
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        await next();
    }
}