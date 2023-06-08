namespace Master.Utilities.Extentions;

using Microsoft.AspNetCore.Http;

public static class HttpContextExtentions
{
    public static string GetClaim(this HttpContext source, string claimType) =>
         source?
        .User?
        .Claims
        .FirstOrDefault(_ => _.Type == claimType)?
        .Value ?? String.Empty;
}
