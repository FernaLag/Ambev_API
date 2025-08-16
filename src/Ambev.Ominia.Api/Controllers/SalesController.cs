using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.Ominia.Application.Features.Sales.Commands.CancelItem;
using Ambev.Ominia.Application.Features.Sales.Commands.CancelSale;
using Ambev.Ominia.Application.Features.Sales.Commands.CreateSale;
using Ambev.Ominia.Application.Features.Sales.Commands.DeleteSale;
using Ambev.Ominia.Application.Features.Sales.Commands.UpdateSale;
using Ambev.Ominia.Application.Features.Sales.Queries.GetSale;
using Ambev.Ominia.Application.Features.Sales.Queries.ListSales;
using Ambev.Ominia.Domain.Responses;
using Ambev.Ominia.Crosscutting.Security;
using Ambev.Ominia.Api.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;

namespace Ambev.Ominia.Api.Controllers;

[RequireAuth]
public class SalesController(IMediator mediator) : ApiController
{
    /// <summary>
    /// Retrieves a paginated list of sales.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse), StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(ListSalesQuery), typeof(ListSalesQueryExample))]
    public async Task<IActionResult> ListSales([FromQuery] ListSalesQuery query)
    {
        var response = await mediator.Send(query);
        return OkPaginated(new PaginatedResponse(response, query.Page, query.PageSize));
    }

    /// <summary>
    /// Retrieves a sale by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSale([FromRoute] int id)
    {
        var response = await mediator.Send(new GetSaleQuery(id));
        return Ok(response);
    }

    /// <summary>
    /// Creates a new sale.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [SwaggerRequestExample(typeof(CreateSaleCommand), typeof(CreateSaleCommandExample))]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
    {
        var response = await mediator.Send(command);
        return Created(Uri, response);
    }

    /// <summary>
    /// Updates an existing sale.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    [SwaggerRequestExample(typeof(UpdateSaleCommand), typeof(UpdateSaleCommandExample))]
    public async Task<IActionResult> UpdateSale([FromRoute] int id, [FromBody] UpdateSaleCommand command)
    {
        command.Id = id;
        var response = await mediator.Send(command);
        return Ok(response);
    }

    /// <summary>
    /// Deletes a sale.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSale([FromRoute] int id)
    {
        await mediator.Send(new DeleteSaleCommand(id));
        return Ok("Sale deleted successfully");
    }

    /// <summary>
    /// Cancels a sale.
    /// </summary>
    [HttpPost("{id}/cancel")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelSale([FromRoute] int id)
    {
        var response = await mediator.Send(new CancelSaleCommand(id));
        return Ok(response);
    }

    /// <summary>
    /// Cancels a specific item in a sale.
    /// </summary>
    [HttpPost("{saleId}/items/{itemId}/cancel")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelItem([FromRoute] int saleId, [FromRoute] int itemId)
    {
        var response = await mediator.Send(new CancelItemCommand(saleId, itemId));
        return Ok(response);
    }
}