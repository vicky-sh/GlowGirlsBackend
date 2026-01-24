using GlowGirlsBackend.Interfaces;
using GlowGirlsBackend.Utility;
using Google.Apis.Calendar.v3.Data;
using Mediator;

namespace GlowGirlsBackend.Features.Appointments;

public class CreateCalendarEventCommandHandler(IGoogleCalendarService googleCalendarService)
    : ICommandHandler<CreateCalendarEventCommand, Result<Event>>
{
    public async Task<Result<Event>> Handle(
        CreateCalendarEventCommand request,
        CancellationToken cancellationToken
    )
    {
        return await googleCalendarService.CreateEvent(null, cancellationToken);
    }
}

public class CreateCalendarEventCommand : ICommand<Result<Event>>;
