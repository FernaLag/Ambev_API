using Ambev.Ominia.Domain.Events;

namespace Ambev.Ominia.Domain.Interfaces.Infrastructure;

/// <summary>
/// Defines the event store repository contract.
/// </summary>
public interface IEventRepository
{
    Task SaveAsync(IEventModel @event);
    Task<List<IEventModel>> FindByAggregateIdAsync(Guid aggregateId);
    Task<List<IEventModel>> FindAllAsync();
}