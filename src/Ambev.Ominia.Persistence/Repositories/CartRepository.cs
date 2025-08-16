using Ambev.Ominia.Domain.Entities.Cart;
using Ambev.Ominia.Domain.Extensions;
using Ambev.Ominia.Domain.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Ambev.Ominia.Persistence.Repositories;

/// <summary>
/// Implementation of ICartRepository using Entity Framework Core.
/// Provides methods for handling cart persistence.
/// </summary>
/// <remarks>
/// Initializes a new instance of CartRepository.
/// </remarks>
/// <param name="context">The database context.</param>
public class CartRepository(PostgreeContext context) : ICartRepository
{

    /// <inheritdoc />
    public async Task<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        await context.Carts.AddAsync(cart, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return cart;
    }

    /// <inheritdoc />
    public async Task<Cart> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"Cart with ID {id} not found");
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        context.Carts.Update(cart);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var cart = await context.Carts.FindAsync([id], cancellationToken)
            ?? throw new KeyNotFoundException($"Cart with ID {id} not found");

        context.Carts.Remove(cart);

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<Cart>> ListAsync(int page, int size, string? order, CancellationToken cancellationToken = default)
    {
        var query = context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).AsQueryable();

        query = query.ApplySorting(order);

        return await query.Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken);
    }


}