using Ambev.Ominia.Domain.Common;
using Ambev.Ominia.Domain.Entities.Sales;
using MongoDB.Bson.Serialization.Attributes;

namespace Ambev.Ominia.Domain.Events.Sales;

/// <summary>
/// Event triggered when a sale is modified.
/// </summary>
public class SaleModifiedEvent : BaseEvent
{
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
    /// Gets the branch where the sale was modified.
    /// </summary>
    [BsonElement("Branch")]
    public string Branch { get; }

    /// <summary>
    /// Gets the updated list of items in the sale.
    /// </summary>
    [BsonElement("Items")]
    public List<SaleItem> Items { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="SaleModifiedEvent"/>.
    /// </summary>
    public SaleModifiedEvent(Guid aggregateId, string saleNumber, DateTime date, string customer, string branch, List<SaleItem> items)
        : base(nameof(SaleModifiedEvent))
    {
        Id = Guid.NewGuid();
        AggregateId = aggregateId;
        SaleNumber = saleNumber;
        Date = date;
        Customer = customer;
        Branch = branch;
        Items = items;
    }
}