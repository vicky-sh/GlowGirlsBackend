namespace GlowGirlsBackend.Middleware;

public class ClientSecretMiddleware(RequestDelegate next, IConfiguration configuration)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var environment = context.RequestServices.GetRequiredService<IWebHostEnvironment>();

        // Allow Swagger UI in Development mode
        if (
            environment.IsDevelopment()
            && (
                context.Request.Path.StartsWithSegments("/swagger")
                || context.Request.Path.StartsWithSegments("/openapi")
                || context.Request.Path.StartsWithSegments("/hangfire")
            )
        )
        {
            await next(context);
            return;
        }

        var expectedSecret = configuration["ClientSecret"];

        if (
            !context.Request.Headers.TryGetValue("GlowGirls-Client-Secret", out var secret)
            || secret != expectedSecret
        )
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid or missing secret.");
            return;
        }

        await next(context);
    }
}
