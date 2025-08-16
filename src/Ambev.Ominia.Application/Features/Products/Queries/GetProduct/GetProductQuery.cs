namespace Ambev.Ominia.Application.Features.Products.Queries.GetProduct;

/// <summary>
/// Command for retrieving a product by its ID.
/// </summary>
/// <remarks>
/// This command is used to fetch the details of an existing product
/// using its Id. It implements <see cref="IRequest{TResponse}"/>
/// to initiate the request that returns a <see cref="ProductDto"/>.
/// </remarks>
public record GetProductQuery : IRequest<ProductDto>
{
    /// <summary>
    /// The Id of the product to retrieve.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Initializes a new instance of GetProductCommand.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>
    public GetProductQuery(int id)
    {
        Id = id;
    }
}