using Ambev.Ominia.Domain.Entities.Cart;

namespace Ambev.Ominia.Domain.Interfaces.Infrastructure;

/// <summary>
/// Repository interface for Cart entity operations.
/// </summary>
public interface ICartRepository
{
    /// <summary>
    /// Creates a new cart in the repository.
    /// </summary>
    /// <param name="cart">The cart to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created cart.</returns>
    Task<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a cart by its Id.
    /// </summary>
    /// <param name="id">The Id of the cart.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The cart if found, null otherwise.</returns>
    Task<Cart> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing cart in the repository.
    /// </summary>
    /// <param name="cart">The cart entity with updated values.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated cart.</returns>
    Task UpdateAsync(Cart cart, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a cart from the repository.
    /// </summary>
    /// <param name="id">The Id of the cart to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the cart was deleted, false if not found.</returns>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of carts with optional filtering and sorting.
    /// </summary>
    /// <param name="page">The page number for pagination.</param>
    /// <param name="size">The number of items per page.</param>
    /// <param name="order">The sorting criteria (e.g., "id desc, userId asc").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of carts that match the criteria.</returns>
    Task<List<Cart>> ListAsync(int page, int size, string? order, CancellationToken cancellationToken = default);
}