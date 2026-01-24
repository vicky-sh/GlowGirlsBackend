using GlowGirlsBackend.Middleware;

namespace GlowGirlsBackend.Extensions;

public static class ClientSecretMiddlewareExtension
{
    public static void UseClientSecretValidation(this IApplicationBuilder app)
    {
        app.UseMiddleware<ClientSecretMiddleware>();
    }
}
