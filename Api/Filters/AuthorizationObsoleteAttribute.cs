using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Api.Filters;

[Obsolete("Old auth verification should be deleted")]
public class AuthorizationObsoleteAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("X-Token", out var userToken))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        using var scope = context.HttpContext.RequestServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var isUserExist = await dbContext.Accounts.AnyAsync(ac => ac.Token == userToken.ToString());

        if (!isUserExist)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        await next();
    }
}