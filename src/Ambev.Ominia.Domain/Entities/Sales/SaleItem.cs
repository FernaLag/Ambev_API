using Ambev.Ominia.Domain.Common;
using Ambev.Ominia.Domain.Entities.Products;
using Ambev.Ominia.Domain.Enums;

namespace Ambev.Ominia.Domain.Entities.Sales;

public class SaleItem : BaseEntity
{
    /// <summary>
    /// Protected constructor for EF Core and AutoMapper.
    /// </summary>
    protected SaleItem() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaleItem"/> class.
    /// </summary>
    public SaleItem(int saleId, int productId, int quantity, decimal unitPrice, decimal discount, int id = 0)
    {
        Id = id;
        SaleId = saleId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        Status = SaleStatus.Active;
    }
    /// <summary>
    /// Foreign key to the related Sale.
    /// </summary>
    public int SaleId { get; private set; }

    /// <summary>
    /// Foreign key to the related Product.
    /// </summary>
    public int ProductId { get; private set; }

    /// <summary>
    /// Navigation property to the related Product.
    /// </summary>
    public Product? Product { get; set; }

    /// <summary>
    /// Quantity of the product sold.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Unit price of the product at the time of sale.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Discount applied to the item.
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Total cost of the item (calculated as UnitPrice * Quantity - Discount).
    /// </summary>
    public decimal Total => UnitPrice * Quantity - Discount;

    /// <summary>
    /// Status of the item (Active or Cancelled).
    /// </summary>
    public SaleStatus Status { get; private set; }

    /// <summary>
    /// Cancels the item.
    /// </summary>
    public void Cancel()
    {
        Status = SaleStatus.Cancelled;
    }

    /// <summary>
    /// Determines if the item is active.
    /// </summary>
    /// <returns>True if the item is active, otherwise false.</returns>
    public bool IsActive()
    {
        return Status == SaleStatus.Active;
    }
}