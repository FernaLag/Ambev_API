using Ambev.Ominia.Domain.Entities.Sales;

namespace Ambev.Ominia.Domain.Services;

/// <summary>
/// Service responsible for calculating discounts based on business rules.
/// </summary>
public static class SaleDiscountService
{
    /// <summary>
    /// Calculates the discount percentage based on quantity.
    /// Business Rules:
    /// - 4-9 items: 10% discount
    /// - 10-20 items: 20% discount
    /// - Less than 4 items: No discount
    /// - More than 20 items: Not allowed
    /// </summary>
    /// <param name="quantity">The quantity of items</param>
    /// <returns>The discount percentage (0, 10, or 20)</returns>
    public static decimal CalculateDiscountPercentage(int quantity)
    {
        if (quantity is >= 10 and <= 20)
            return 20;
        if (quantity >= 4)
            return 10;
        return 0;
    }

    /// <summary>
    /// Calculates the discount amount for a sale item.
    /// </summary>
    /// <param name="saleItem">The sale item</param>
    /// <returns>The discount amount</returns>
    public static decimal CalculateDiscountAmount(SaleItem saleItem)
    {
        var discountPercentage = CalculateDiscountPercentage(saleItem.Quantity);
        var totalPrice = saleItem.Quantity * saleItem.UnitPrice;
        return totalPrice * (discountPercentage / 100);
    }

    /// <summary>
    /// Validates that the quantity doesn't exceed the maximum allowed.
    /// </summary>
    /// <param name="quantity">The quantity to validate</param>
    /// <exception cref="InvalidOperationException">Thrown when quantity exceeds 20</exception>
    public static void ValidateMaxQuantity(int quantity)
    {
        if (quantity > 20)
            throw new InvalidOperationException("Cannot sell more than 20 identical items");
    }

    /// <summary>
    /// Validates that discount is not applied to items with quantity below 4.
    /// </summary>
    /// <param name="quantity">The quantity of items</param>
    /// <param name="discount">The discount amount</param>
    /// <exception cref="InvalidOperationException">Thrown when discount is applied to less than 4 items</exception>
    public static void ValidateDiscountEligibility(int quantity, decimal discount)
    {
        if (quantity < 4 && discount > 0)
            throw new InvalidOperationException("Discount is not allowed for purchases below 4 items");
    }

    /// <summary>
    /// Calculates the total amount for a sale applying all discount rules.
    /// </summary>
    /// <param name="sale">The sale to calculate</param>
    /// <returns>The total amount with discounts applied</returns>
    public static decimal CalculateSaleTotal(Sale sale)
    {
        decimal total = 0;
        foreach (var item in sale.Items)
        {
            ValidateMaxQuantity(item.Quantity);
            ValidateDiscountEligibility(item.Quantity, item.Discount);
            
            var itemTotal = item.Quantity * item.UnitPrice;
            var discountAmount = CalculateDiscountAmount(item);
            total += itemTotal - discountAmount;
        }
        return total;
    }
}