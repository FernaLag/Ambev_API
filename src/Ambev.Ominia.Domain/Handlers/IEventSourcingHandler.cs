using Ambev.Ominia.Domain.Common;

namespace Ambev.Ominia.Domain.Handlers;
public interface IEventSourcingHandler<TAggregate>
{
    Task SaveAsync(AggregateRoot aggregate);
    Task<TAggregate> GetByIdAsync(Guid aggregateId);
    Task<Dictionary<Guid, List<BaseEvent>>> RepublishEventsAsync();
}