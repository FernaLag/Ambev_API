using Ambev.Ominia.Domain.Entities.Sales;

namespace Ambev.Ominia.Domain.Interfaces.Infrastructure;

/// <summary>
/// Repository interface for Sale entity operations.
/// </summary>
public interface ISaleRepository
{
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);
    Task<Sale> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Sale>> ListAsync(int page, int size, string? order, CancellationToken cancellationToken = default);
}