using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.Ominia.Application.Features.Carts.Commands.CreateCart;
using Ambev.Ominia.Application.Features.Carts.Commands.DeleteCart;
using Ambev.Ominia.Application.Features.Carts.Commands.UpdateCart;
using Ambev.Ominia.Application.Features.Carts.Queries.ListCarts;
using Ambev.Ominia.Application.Features.Carts.Queries.GetCart;
using Ambev.Ominia.Domain.Responses;
using Ambev.Ominia.Crosscutting.Security;
using Ambev.Ominia.Api.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;

namespace Ambev.Ominia.Api.Controllers;

[RequireAuth]
public class CartsController(IMediator mediator) : ApiController
{
    /// <summary>
    /// Retrieves a paginated list of carts.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse), StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(ListCartsQuery), typeof(ListCartsQueryExample))]
    public async Task<IActionResult> ListCarts([FromQuery] ListCartsQuery query)
    {
        var response = await mediator.Send(query);
        return Ok(new PaginatedResponse(response));
    }

    /// <summary>
    /// Retrieves a specific cart by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCart([FromRoute] int id)
    {
        var response = await mediator.Send(new GetCartQuery(id));
        return Ok(new Response(response));
    }

    /// <summary>
    /// Creates a new cart.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [SwaggerRequestExample(typeof(CreateCartCommand), typeof(CreateCartCommandExample))]
    public async Task<IActionResult> CreateCart([FromBody] CreateCartCommand command)
    {
        var response = await mediator.Send(command);
        return Created(Uri, response);
    }

    /// <summary>
    /// Updates an existing cart.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    [SwaggerRequestExample(typeof(UpdateCartCommand), typeof(UpdateCartCommandExample))]
    public async Task<IActionResult> UpdateCart([FromRoute] int id, [FromBody] UpdateCartCommand command)
    {
        command.Id = id;
        var response = await mediator.Send(command);
        return Ok(new Response(response));
    }

    /// <summary>
    /// Deletes a cart.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCart([FromRoute] int id)
    {
        await mediator.Send(new DeleteCartCommand(id));
        return Ok("Cart deleted successfully");
    }
}