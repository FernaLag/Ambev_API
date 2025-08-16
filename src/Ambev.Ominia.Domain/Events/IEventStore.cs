using Ambev.Ominia.Domain.Common;

namespace Ambev.Ominia.Domain.Events;

public interface IEventService<TAggregate> where TAggregate : AggregateRoot
{
    Task<List<Guid>> GetAggregateIdsAsync();
    Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);
    Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
}