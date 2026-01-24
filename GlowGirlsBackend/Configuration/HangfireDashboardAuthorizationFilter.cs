using System.Text;
using Hangfire.Dashboard;

namespace GlowGirlsBackend.Configuration;

public class HangfireDashboardAuthorizationFilter(IConfiguration configuration)
    : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var http = context.GetHttpContext();

        if (
            http.Request.Headers.TryGetValue("Authorization", out var authHeader)
            && authHeader.ToString().StartsWith("Basic ", StringComparison.OrdinalIgnoreCase)
        )
        {
            var encoded = authHeader.ToString()["Basic ".Length..];
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
            var parts = decoded.Split(':', 2);

            if (
                parts.Length == 2
                && parts[0] == configuration["HangfireDashboardCredentials:Username"]
                && parts[1] == configuration["HangfireDashboardCredentials:Password"]
            )
                return true;
        }

        http.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Hangfire Dashboard\"";
        http.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return false;
    }
}
