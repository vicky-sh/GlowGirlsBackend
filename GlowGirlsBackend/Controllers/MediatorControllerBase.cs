using GlowGirlsBackend.Utility;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace GlowGirlsBackend.Controllers;

public class MediatorControllerBase(IMediator mediator) : ControllerBase
{
    protected async Task<ActionResult<TResponse>> HandleCommandAsync<TResponse>(
        IRequest<TResponse> command,
        CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    protected async Task<ActionResult<TResponse>> HandleCommandAsync<TResponse>(
        IRequest<Result<TResponse>> command,
        CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(command, cancellationToken);
        return response.Succeeded ? Ok(response) : HandleError(response.Errors);
    }

    private ActionResult HandleError(List<Error> errors)
    {
        var error = errors.First();
        var httpCode = MapErrorCodeToStatus(error.Code);
        return StatusCode(httpCode, errors);
    }

    private static int MapErrorCodeToStatus(int code)
    {
        return code switch
        {
            1000 => StatusCodes.Status400BadRequest,
            2000 => StatusCodes.Status404NotFound,
            3000 => StatusCodes.Status401Unauthorized,
            4000 => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}