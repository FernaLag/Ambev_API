namespace Ambev.Ominia.Application.Features.Products.Commands.DeleteProduct;

/// <summary>
/// Command for deleting a product.
/// </summary>
/// <remarks>
/// This command is used to delete an existing product by its Id.
/// It implements <see cref="IRequest"/> to initiate the request 
/// </remarks>
public record DeleteProductCommand : IRequest
{
    /// <summary>
    /// The Id of the product to delete.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Initializes a new instance of DeleteProductCommand.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    public DeleteProductCommand(int id)
    {
        Id = id;
    }
}