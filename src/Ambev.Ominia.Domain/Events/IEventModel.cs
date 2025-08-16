using Ambev.Ominia.Domain.Common;

namespace Ambev.Ominia.Domain.Events;

/// <summary>
/// Defines a contract for an event model in the event store.
/// </summary>
public interface IEventModel
{
    string Id { get; }
    DateTime TimeStamp { get; }
    Guid AggregateIdentifier { get; }
    string AggregateType { get; }
    int Version { get; }
    string EventType { get; }
    BaseEvent EventData { get; }
}