using MongoDB.Bson.Serialization.Attributes;

namespace Ambev.Ominia.Domain.Common;

public abstract class BaseEvent(string type)
    {
    [BsonElement("EventId")]
    public Guid Id { get; set; }

    [BsonElement("AggregateId")]
    public Guid AggregateId { get; set; }

    public string Type { get; set; } = type;

    public int Version { get; set; }
}