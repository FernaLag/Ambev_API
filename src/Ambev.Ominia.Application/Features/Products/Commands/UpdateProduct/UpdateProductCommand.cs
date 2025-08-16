using Ambev.Ominia.Domain.ValueObjects;

namespace Ambev.Ominia.Application.Features.Products.Commands.UpdateProduct;

/// <summary>
/// Command for updating an existing product.
/// </summary>
/// <remarks>
/// This command is used to update an existing product by its Id.
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request
/// that returns a <see cref="UpdateProductDto"/>.
/// </remarks>
public class UpdateProductCommand : IRequest<ProductDto>
{
    /// <summary>
    /// The Id of the product to update.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The updated title of the product.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The updated price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// The updated description of the product.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The updated category of the product.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// The updated product image URL.
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// The updated rating details of the product.
    /// </summary>
    public Rating Rating { get; set; } = new Rating(0, 0);
}