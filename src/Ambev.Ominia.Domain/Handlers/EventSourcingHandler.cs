using Ambev.Ominia.Domain.Common;
using Ambev.Ominia.Domain.Events;
using Ambev.Ominia.Domain.Interfaces;

namespace Ambev.Ominia.Domain.Handlers;

/// <summary>
/// Handles event sourcing operations, including retrieving, saving, and republishing events.
/// </summary>
/// <typeparam name="TAggregate">The aggregate type for event sourcing.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="EventSourcingHandler{TAggregate}"/> class.
/// </remarks>
/// <param name="eventStore">The event store to persist and retrieve events.</param>
public class EventSourcingHandler<TAggregate>(IEventService<TAggregate> eventStore) : IEventSourcingHandler<TAggregate> where TAggregate : AggregateRoot, new()
{
    /// <summary>
    /// Retrieves an aggregate by its ID and replays its events.
    /// </summary>
    /// <param name="aggregateId">The unique identifier of the aggregate.</param>
    /// <returns>The reconstructed aggregate.</returns>
    public async Task<TAggregate> GetByIdAsync(Guid aggregateId)
    {
        var aggregate = new TAggregate();
        var events = await eventStore.GetEventsAsync(aggregateId);

        if (events == null || events.Count == 0) return aggregate;

        aggregate.ReplayEvents(events);
        aggregate.Version = events.Max(e => e.Version);

        return aggregate;
    }

    /// <summary>
    /// Retrieves and returns all stored events for external republishing.
    /// </summary>
    /// <returns>A dictionary where the key is the aggregate ID and the value is the list of events.</returns>
    public async Task<Dictionary<Guid, List<BaseEvent>>> RepublishEventsAsync()
    {
        var aggregateIds = await eventStore.GetAggregateIdsAsync();
        var republishEvents = new Dictionary<Guid, List<BaseEvent>>();

        if (aggregateIds == null || aggregateIds.Count == 0) return republishEvents;

        foreach (var aggregateId in aggregateIds)
        {
            var aggregate = await GetByIdAsync(aggregateId);

            if (aggregate is IHasStatus statusAggregate && !statusAggregate.IsActive()) continue;

            var events = await eventStore.GetEventsAsync(aggregateId);

            if (events.Count != 0)
            {
                republishEvents.Add(aggregateId, events);
            }
        }

        return republishEvents;
    }

    /// <summary>
    /// Saves an aggregate's uncommitted events to the event store.
    /// </summary>
    /// <param name="aggregate">The aggregate containing uncommitted changes.</param>
    public async Task SaveAsync(AggregateRoot aggregate)
    {
        await eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
        aggregate.MarkChangesAsCommitted();
    }
}