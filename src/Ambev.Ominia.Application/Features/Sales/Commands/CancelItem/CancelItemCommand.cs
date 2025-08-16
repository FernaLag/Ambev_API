namespace Ambev.Ominia.Application.Features.Sales.Commands.CancelItem;

/// <summary>
/// Command to cancel a specific item in a sale.
/// </summary>
public record CancelItemCommand : IRequest<SaleDto>
{
    /// <summary>
    /// The ID of the sale containing the item.
    /// </summary>
    public int SaleId { get; }

    /// <summary>
    /// The ID of the item to cancel.
    /// </summary>
    public int ItemId { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="CancelItemCommand"/>.
    /// </summary>
    /// <param name="saleId">The ID of the sale.</param>
    /// <param name="itemId">The ID of the item to cancel.</param>
    public CancelItemCommand(int saleId, int itemId)
    {
        SaleId = saleId;
        ItemId = itemId;
    }
}