using Ambev.Ominia.Domain.Entities.Products;

namespace Ambev.Ominia.Domain.Interfaces.Infrastructure;

/// <summary>
/// Repository interface for Product entity operations.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Creates a new product in the repository.
    /// </summary>
    /// <param name="product">The product to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created product.</returns>
    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a product by its Id.
    /// </summary>
    /// <param name="id">The Id of the product.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The product if found, null otherwise.</returns>
    Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing product in the repository.
    /// </summary>
    /// <param name="product">The product entity with updated values.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated product.</returns>
    Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a product from the repository.
    /// </summary>
    /// <param name="id">The Id of the product to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the product was deleted, false if not found.</returns>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of products with optional filtering and sorting.
    /// </summary>
    /// <param name="page">The page number for pagination.</param>
    /// <param name="size">The number of items per page.</param>
    /// <param name="order">The sorting criteria (e.g., "price desc, title asc").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of products that match the criteria.</returns>
    Task<List<Product>> ListAsync(int page, int size, string? order, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all available product categories.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of unique product categories.</returns>
    Task<List<string>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of products filtered by category.
    /// </summary>
    /// <param name="category">The category name to filter by.</param>
    /// <param name="page">The page number for pagination.</param>
    /// <param name="size">The number of items per page.</param>
    /// <param name="order">The sorting criteria (e.g., "price desc, title asc").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of products within the specified category.</returns>
    Task<List<Product>> ListByCategoryAsync(string category, int page, int size, string? order, CancellationToken cancellationToken = default);
}