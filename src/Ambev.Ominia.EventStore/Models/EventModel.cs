using Ambev.Ominia.Domain.Events;
using Ambev.Ominia.Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ambev.Ominia.EventStore.Models;

/// <summary>
/// Represents an event stored in the event store.
/// </summary>
public class EventModel : IEventModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }

    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

    public Guid AggregateIdentifier { get; set; }

    public required string AggregateType { get; set; }

    public int Version { get; set; }

    public required string EventType { get; set; }

    public required BaseEvent EventData { get; set; }
}