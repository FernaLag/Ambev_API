using Ambev.Ominia.Domain.Common;
using Ambev.Ominia.Domain.Enums;

namespace Ambev.Ominia.Domain.Entities.Sales;

/// <summary>
/// Represents a sale transaction.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Unique identifier for the sale.
    /// </summary>
    public string SaleNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Date of the sale.
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Customer name.
    /// </summary>
    public string Customer { get; private set; } = string.Empty;

    /// <summary>
    /// Branch name.
    /// </summary>
    public string Branch { get; private set; } = string.Empty;

    /// <summary>
    /// Collection of sale items.
    /// </summary>
    public List<SaleItem> Items { get; private set; } = [];

    /// <summary>
    /// Total amount of the sale.
    /// </summary>
    public decimal TotalAmount => Items.Where(i => i.IsActive()).Sum(i => i.Total);

    /// <summary>
    /// Sale status.
    /// </summary>
    public SaleStatus Status { get; private set; }

    /// <summary>
    /// Protected constructor for EF Core and AutoMapper.
    /// </summary>
    protected Sale() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Sale"/> class.
    /// </summary>
    public Sale(string saleNumber, DateTime date, string customer, string branch, List<SaleItem> items)
    {
        SaleNumber = string.IsNullOrWhiteSpace(saleNumber)
            ? throw new ArgumentException("Sale number cannot be empty.", nameof(saleNumber))
            : saleNumber;

        Date = date;
        Customer = string.IsNullOrWhiteSpace(customer)
            ? throw new ArgumentException("Customer name cannot be empty.", nameof(customer))
            : customer;

        Branch = string.IsNullOrWhiteSpace(branch)
            ? throw new ArgumentException("Branch name cannot be empty.", nameof(branch))
            : branch;

        Items = items ?? [];
        Status = SaleStatus.Active;
    }

    /// <summary>
    /// Updates the sale details.
    /// </summary>
    public void Update(string saleNumber, DateTime date, string customer, string branch, List<SaleItem> items)
    {
        SaleNumber = saleNumber;
        Date = date;
        Customer = customer;
        Branch = branch;
        Items = items;
    }

    /// <summary>
    /// Cancels the sale.
    /// </summary>
    public void Cancel()
    {
        Status = SaleStatus.Cancelled;
    }

    /// <summary>
    /// Cancels a specific item in the sale.
    /// </summary>
    /// <param name="itemId">The ID of the item to cancel.</param>
    /// <returns>True if the item was found and cancelled, false otherwise.</returns>
    public bool CancelItem(int itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null) return false;

        item.Cancel();
        return true;
    }

    public bool IsActive()
    {
        return Status == SaleStatus.Active;
    }
}