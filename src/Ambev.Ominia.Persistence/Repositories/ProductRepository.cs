using Ambev.Ominia.Domain.Entities.Products;
using Ambev.Ominia.Domain.Extensions;
using Ambev.Ominia.Domain.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Ambev.Ominia.Persistence.Repositories;

/// <summary>
/// Implementation of IProductRepository using Entity Framework Core.
/// Provides methods for handling product persistence.
/// </summary>
/// <remarks>
/// Initializes a new instance of ProductRepository.
/// </remarks>
/// <param name="context">The database context.</param>
public class ProductRepository(PostgreeContext context) : IProductRepository
{

    /// <inheritdoc />
    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await context.Products.AddAsync(product, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return product;
    }

    /// <inheritdoc />
    public async Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Products.FindAsync([id], cancellationToken)
            ?? throw new KeyNotFoundException($"Product with ID {id} not found");
    }

    /// <inheritdoc />
    public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync(cancellationToken);
        return product;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        context.Products.Remove(await GetByIdAsync(id, cancellationToken));
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<Product>> ListAsync(int page, int size, string? order, CancellationToken cancellationToken = default)
    {
        var query = context.Products.AsQueryable();

        query = query.ApplySorting(order, q => q.OrderBy(p => p.Id));

        return await query.Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<string>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await context.Products
            .Select(p => p.Category)
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<Product>> ListByCategoryAsync(string category, int page, int size, string? order, CancellationToken cancellationToken = default)
    {
        var query = context.Products.Where(p => p.Category.Contains(category));

        query = query.ApplySorting(order, q => q.OrderBy(p => p.Id));

        return await query.Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken);
    }


}