using GlowGirlsBackend.Features.Appointments;
using Google.Apis.Calendar.v3.Data;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace GlowGirlsBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController(IMediator mediator) : MediatorControllerBase(mediator)
{
    [HttpPost]
    public async Task<ActionResult<Event>> CreateAppointment(CancellationToken cancellationToken)
    {
        return await HandleCommandAsync(new CreateCalendarEventCommand(), cancellationToken);
    }

    [HttpGet]
    public Task GetAppointments()
    {
        return Task.FromResult<IActionResult>(Ok());
    }
}
