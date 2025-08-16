using Ambev.Ominia.Domain.Common;
using Ambev.Ominia.Domain.Events;
using Ambev.Ominia.Domain.Exceptions;
using Ambev.Ominia.Domain.Interfaces.Infrastructure;
using Ambev.Ominia.EventStore.Models;
using Rebus.Bus;

namespace Ambev.Ominia.EventStore.Services;

public class EventService<TAggregate>(IEventRepository eventStoreRepository, IBus bus) : IEventService<TAggregate> where TAggregate : AggregateRoot
{
    public async Task<List<Guid>> GetAggregateIdsAsync()
    {
        var eventStream = await eventStoreRepository.FindAllAsync();

        if (eventStream == null || eventStream.Count == 0)
            throw new ArgumentNullException(nameof(eventStream), "Could not retrieve event stream from the event store!");

        return [.. eventStream.Select(x => x.AggregateIdentifier).Distinct()];
    }

    public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
    {
        var eventStream = await eventStoreRepository.FindByAggregateIdAsync(aggregateId);

        if (eventStream == null || eventStream.Count == 0)
            return new List<BaseEvent>();

        return [.. eventStream.OrderBy(x => x.Version).Select(x => x.EventData)];
    }

    public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
    {
        var eventStream = await eventStoreRepository.FindByAggregateIdAsync(aggregateId);

        if (expectedVersion != -1 && eventStream.Count != 0 && eventStream[^1].Version != expectedVersion)
            throw new ConcurrencyException();

        var version = expectedVersion;

        foreach (var @event in events)
        {
            version++;
            @event.Version = version;
            var eventType = @event.GetType().Name;
            var eventModel = new EventModel
            {
                Id = Guid.NewGuid().ToString(),
                TimeStamp = DateTime.Now,
                AggregateIdentifier = aggregateId,
                AggregateType = typeof(TAggregate).Name,
                Version = version,
                EventType = eventType,
                EventData = @event
            };

            await eventStoreRepository.SaveAsync(eventModel);
            await bus.Publish(@event);
        }
    }
}