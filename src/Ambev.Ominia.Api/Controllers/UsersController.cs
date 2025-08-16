using Ambev.Ominia.Api.SwaggerExamples;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.Ominia.Application.Features.Users.Commands.CreateUser;
using Ambev.Ominia.Application.Features.Users.Commands.DeleteUser;
using Ambev.Ominia.Application.Features.Users.Queries.GetUser;
using Ambev.Ominia.Domain.Responses;
using Ambev.Ominia.Crosscutting.Security;
using Swashbuckle.AspNetCore.Filters;

namespace Ambev.Ominia.Api.Controllers;

[RequireAuth]
public class UsersController(IMediator mediator) : ApiController
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="command">The user creation command.</param>
    /// <returns>The created user details.</returns>
    [HttpPost]
    [SwaggerRequestExample(typeof(CreateUserCommand), typeof(CreateUserCommandExample))]
    [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var response = await mediator.Send(command);
        return Created(Uri, response);
    }

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="id">The Id of the user.</param>
    /// <returns>The user details if found.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser([FromRoute] int id)
    {
        var response = await mediator.Send(new GetUserQuery(id));
        return Ok(response);
    }

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="id">The Id of the user to delete.</param>
    /// <returns>Success response if the user was deleted.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        await mediator.Send(new DeleteUserCommand(id));
        return Ok("User deleted successfully");
    }
}