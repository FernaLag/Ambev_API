using Ambev.Ominia.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace Ambev.Ominia.Domain.Events.Sales;

/// <summary>
/// Event triggered when an item in a sale is cancelled.
/// </summary>
public class ItemCancelledEvent : BaseEvent
{
    /// <summary>
    /// Gets the item ID that was cancelled.
    /// </summary>
    [BsonElement("ItemId")]
    public int ItemId { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ItemCancelledEvent"/>.
    /// </summary>
    public ItemCancelledEvent(Guid aggregateId, int itemId)
        : base(nameof(ItemCancelledEvent))
    {
        Id = Guid.NewGuid();
        AggregateId = aggregateId;
        ItemId = itemId;
    }
}