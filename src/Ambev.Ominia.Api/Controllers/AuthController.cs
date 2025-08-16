using Ambev.Ominia.Api.SwaggerExamples;
using Ambev.Ominia.Application.Features.Auth.Commands.Authenticate;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.Ominia.Domain.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace Ambev.Ominia.Api.Controllers;

public class AuthController(IMediator mediator) : ApiController
{
    /// <summary>
    /// Authenticates a user with their credentials
    /// </summary>
    /// <param name="command">The authentication command</param>
    /// <returns>Authentication token if successful</returns>
    [HttpPost]
    [SwaggerRequestExample(typeof(AuthenticateCommand), typeof(AuthenticateCommandExample))]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticateCommand command)
    {
        var response = await mediator.Send(command);

        return Ok(new Response(response));
    }
}