using Ambev.Ominia.Domain.Entities.Sales;
using Ambev.Ominia.Domain.Extensions;
using Ambev.Ominia.Domain.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Ambev.Ominia.Persistence.Repositories;

/// <summary>
/// Implementation of ISaleRepository using Entity Framework Core.
/// Provides methods for handling sale persistence.
/// </summary>
public class SaleRepository(PostgreeContext context) : ISaleRepository
{
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await context.Sales.AddAsync(sale, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        Console.WriteLine("SaleCreated");
        return sale;
    }

    public async Task<Sale> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Sales
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException("Sale with ID {id} not found");
    }

    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        context.Sales.Update(sale);
        await context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        context.Sales.Remove(await GetByIdAsync(id, cancellationToken));
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Sale>> ListAsync(int page, int size, string? order, CancellationToken cancellationToken = default)
    {
        var query = context.Sales
            .AsNoTracking()
            .Include(s => s.Items)
            .AsQueryable();

        query = query.ApplySorting(order);

        return await query.Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken);
    }

    private static readonly HashSet<string> AllowedSortFields = new(StringComparer.OrdinalIgnoreCase)
            { "Id", "SaleNumber", "Date", "Customer", "Branch", "Status" };
}