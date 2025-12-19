using GlowGirlsBackend.Models;
using Google.Apis.Calendar.v3.Data;

namespace GlowGirlsBackend.Interfaces;

public interface IGoogleCalendarService
{
    Task<Event> CreateEvent(CreateCalendarEventDto? request, CancellationToken cancellationToken);
}