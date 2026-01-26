using GlowGirlsBackend.Features.ContactUs;
using GlowGirlsBackend.Models;
using GlowGirlsBackend.Utility;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace GlowGirlsBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactUsController(IMediator mediator) : MediatorControllerBase(mediator)
{
    [HttpPost]
    public async Task<ActionResult<Result>> SendContactUsEmail(
        [FromBody] ContactDto contactDto,
        CancellationToken cancellationToken
    )
    {
        return await HandleCommandAsync(new ContactUsCommand(contactDto), cancellationToken);
    }
}
