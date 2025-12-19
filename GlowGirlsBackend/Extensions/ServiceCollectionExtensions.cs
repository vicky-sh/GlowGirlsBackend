using GlowGirlsBackend.Interfaces;
using GlowGirlsBackend.Models.Settings;
using GlowGirlsBackend.Repository;
using GlowGirlsBackend.Services;
using Hangfire;
using Mediator;

namespace GlowGirlsBackend.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<GoogleServiceAccountDetails>(
            configuration.GetSection("GoogleServiceAccountDetails")
        );

        services.Configure<GmailSettings>(configuration.GetSection("GmailSettings"));

        services.AddScoped<IGoogleCalendarService, GoogleCalendarService>();
        services.AddMediator(typeof(Program).Assembly);

        services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddHttpContextAccessor();

        services.AddHangfire(h =>
            h.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"))
        );

        services.AddHangfireServer();
    }
}