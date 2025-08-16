using Ambev.Ominia.Domain.Common;
using Ambev.Ominia.Domain.Entities.Sales;
using Ambev.Ominia.Domain.Enums;
using Ambev.Ominia.Domain.Events.Sales;
using Ambev.Ominia.Domain.Interfaces;

namespace Ambev.Ominia.Domain.Aggregates;

public class SaleAggregate : AggregateRoot, IHasStatus
{
    public int SaleId { get; private set; }
    public string SaleNumber { get; private set; } = string.Empty;
    public DateTime Date { get; private set; } = DateTime.UtcNow;
    public string Customer { get; private set; } = string.Empty;
    public string Branch { get; private set; } = string.Empty;
    public List<SaleItem> Items { get; private set; } = new();
    public SaleStatus Status { get; private set; } = SaleStatus.Active;

    // Required by EF Core
    public SaleAggregate() { }

    private SaleAggregate(Guid id, int saleId, string saleNumber, DateTime date, string customer, string branch, List<SaleItem> items)
    {
        _id = id;
        RaiseEvent(new SaleCreatedEvent(id, saleId, saleNumber, date, customer, branch, items ?? new()));
    }

    public static SaleAggregate Create(Guid id, int saleId, string saleNumber, DateTime date, string customer, string branch, List<SaleItem> items)
        => new(id, saleId, saleNumber, date, customer, branch, items);

    public bool IsActive() => Status == SaleStatus.Active;

    public void UpdateSale(string saleNumber, DateTime date, string customer, string branch, List<SaleItem> items)
    {
        RaiseEvent(new SaleModifiedEvent(Id, saleNumber, date, customer, branch, items ?? new()));
    }

    public void CancelSale()
    {
        if (Status == SaleStatus.Cancelled) return;
        RaiseEvent(new SaleCancelledEvent(Id));
    }

    public void CancelItem(int itemId)
    {
        RaiseEvent(new ItemCancelledEvent(Id, itemId));
    }

    // Apply methods (event -> state)

    public void Apply(SaleCreatedEvent @event)
    {
        _id        = @event.AggregateId;      // aggregate stream id (Guid)
        SaleId     = @event.SaleId;           // numeric id from the read model
        SaleNumber = @event.SaleNumber;
        Date       = @event.Date;
        Customer   = @event.Customer;
        Branch     = @event.Branch;
        Items      = (@event.Items ?? new()).ToList();
        Status     = SaleStatus.Active;
    }

    public void Apply(SaleModifiedEvent @event)
    {
        SaleNumber = @event.SaleNumber;
        Date       = @event.Date;
        Customer   = @event.Customer;
        Branch     = @event.Branch;
        Items      = (@event.Items ?? new()).ToList();
    }

    public void Apply(SaleCancelledEvent @event)
    {
        Status = SaleStatus.Cancelled;
    }

    public void Apply(ItemCancelledEvent @event)
    {
        var item = Items.FirstOrDefault(i => i.Id == @event.ItemId);
        item?.Cancel();
    }
}