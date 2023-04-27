using System.Security.Claims;

namespace Api.Extensions;

public static class ClaimUserExtensions
{
    public static string GetId(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
    }
}