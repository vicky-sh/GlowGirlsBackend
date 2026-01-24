using GlowGirlsBackend.Interfaces;
using GlowGirlsBackend.Models;
using GlowGirlsBackend.Models.Settings;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GlowGirlsBackend.Services;

public class GoogleCalendarService : IGoogleCalendarService
{
    private readonly CalendarService _calendarService;

    public GoogleCalendarService(IOptions<GoogleServiceAccountDetails> calendarSettings)
    {
        var json = JsonConvert.SerializeObject(calendarSettings.Value);

        // TODO: Use the factory to create the credential in future to minimize security risks
#pragma warning disable CS0618 // Type or member is obsolete
        var credential = GoogleCredential
            .FromJson(json)
#pragma warning restore CS0618 // Type or member is obsolete
            .CreateScoped(CalendarService.Scope.Calendar);

        _calendarService = new CalendarService(
            new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "GlowGirlsBackend",
            }
        );
    }

    public async Task<Event> CreateEvent(
        CreateCalendarEventDto? request,
        CancellationToken cancellationToken
    )
    {
        var newEvent = new Event
        {
            Summary = "Vicky's Appointment",
            Location = "Online",
            Description = "Created via Service Account in ASP.NET Core",
            Start = new EventDateTime
            {
                DateTimeDateTimeOffset = DateTimeOffset.UtcNow.AddHours(3),
                TimeZone = "Asia/Kolkata",
            },
            End = new EventDateTime
            {
                DateTimeDateTimeOffset = DateTimeOffset.UtcNow.AddHours(4),
                TimeZone = "Asia/Kolkata",
            },
        };
        var calendarRequest = _calendarService.Events.Insert(
            newEvent,
            "glowgirlsparlour@gmail.com"
        );
        return await calendarRequest.ExecuteAsync(cancellationToken);
    }
}
