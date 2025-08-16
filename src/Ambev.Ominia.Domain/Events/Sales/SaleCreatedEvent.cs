using Ambev.Ominia.Domain.Common;
using Ambev.Ominia.Domain.Entities.Sales;
using MongoDB.Bson.Serialization.Attributes;

namespace Ambev.Ominia.Domain.Events.Sales;

/// <summary>
/// Event triggered when a sale is created.
/// </summary>
public class SaleCreatedEvent : BaseEvent
{
    /// <summary>
    /// Gets the sale ID.
    /// </summary>
    [BsonElement("SaleId")]
    public int SaleId { get; }

    /// <summary>
    /// Gets the sale number.
    /// </summary>
    [BsonElement("SaleNumber")]
    public string SaleNumber { get; }

    /// <summary>
    /// Gets the date of the sale.
    /// </summary>
    [BsonElement("Date")]
    public DateTime Date { get; }

    /// <summary>
    /// Gets the customer associated with the sale.
    /// </summary>
    [BsonElement("Customer")]
    public string Customer { get; }

    /// <summary>
    /// Gets the branch where the sale was made.
    /// </summary>
    [BsonElement("Branch")]
    public string Branch { get; }

    /// <summary>
    /// Gets the list of items in the sale.
    /// </summary>
    [BsonElement("Items")]
    public List<SaleItem> Items { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="SaleCreatedEvent"/>.
    /// </summary>
    public SaleCreatedEvent(Guid aggregateId, int saleId, string saleNumber, DateTime date, string customer, string branch, List<SaleItem> items)
        : base(nameof(SaleCreatedEvent))
    {
        Id = Guid.NewGuid();
        AggregateId = aggregateId;
        SaleId = saleId;
        SaleNumber = saleNumber;
        Date = date;
        Customer = customer;
        Branch = branch;
        Items = items;
    }
}