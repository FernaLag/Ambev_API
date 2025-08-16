using Ambev.Ominia.Api.SwaggerExamples;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.Ominia.Application.Features.Products.Commands.CreateProduct;
using Ambev.Ominia.Application.Features.Products.Commands.DeleteProduct;
using Ambev.Ominia.Application.Features.Products.Commands.UpdateProduct;
using Ambev.Ominia.Application.Features.Products.Queries.GetProduct;
using Ambev.Ominia.Application.Features.Products.Queries.ListProducts;
using Ambev.Ominia.Application.Features.Products.Queries.ListCategories;
using Ambev.Ominia.Domain.Responses;
using Ambev.Ominia.Crosscutting.Security;
using Swashbuckle.AspNetCore.Filters;

namespace Ambev.Ominia.Api.Controllers;

[RequireAuth]
public class ProductsController(IMediator mediator) : ApiController
{
    /// <summary>
    /// Retrieves a paginated list of products.
    /// </summary>
    /// <param name="query">Query parameters for pagination and filtering.</param>
    /// <returns>List of products.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListProducts([FromQuery] ListProductsQuery query)
    {
        var response = await mediator.Send(query);
        return Ok(new PaginatedResponse(response));
    }

    /// <summary>
    /// Retrieves a product by ID.
    /// </summary>
    /// <param name="id">The product ID.</param>
    /// <returns>Product details.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct([FromRoute] int id)
    {
        var response = await mediator.Send(new GetProductQuery(id));
        return Ok(response);
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="command">Product creation command.</param>
    /// <returns>Created product details.</returns>
    [HttpPost]
    [SwaggerRequestExample(typeof(CreateProductCommand), typeof(CreateProductCommandExample))]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        var response = await mediator.Send(command);
        return Created(Uri, response);
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="id">The product ID.</param>
    /// <param name="command">Update command.</param>
    /// <returns>Updated product details.</returns>
    [HttpPut("{id}")]
    [SwaggerRequestExample(typeof(UpdateProductCommand), typeof(UpdateProductCommandExample))]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] UpdateProductCommand command)
    {
        command.Id = id;
        var response = await mediator.Send(command);
        return Ok(response);
    }

    /// <summary>
    /// Deletes a product.
    /// </summary>
    /// <param name="id">The product ID.</param>
    /// <returns>Success status.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct([FromRoute] int id)
    {
        await mediator.Send(new DeleteProductCommand(id));
        return Ok("Product deleted successfully");
    }

    /// <summary>
    /// Retrieves all product categories.
    /// </summary>
    /// <returns>List of product categories.</returns>
    [HttpGet("categories")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListCategories()
    {
        var response = await mediator.Send(new ListCategoriesQuery());
        return Ok(response);
    }
}