using Ambev.Ominia.Domain.Common.Validations;

namespace Ambev.Ominia.Domain.Entities.Sales.Validations;

/// <summary>
/// Provides validation rules for the Sale entity.
/// </summary>
public static class SaleValidation
{
    public static List<ValidationFailure> CanCreateSale(Sale sale)
        => Validation.Init
            .NotNull(sale.SaleNumber, nameof(sale.SaleNumber))
            .NotNull(sale.Customer, nameof(sale.Customer))
            .NotNull(sale.Branch, nameof(sale.Branch))
            .NotEmpty(sale.Items, nameof(sale.Items))
            .Join(ValidateSaleItems(sale.Items));

    private static List<ValidationFailure> ValidateSaleItems(List<SaleItem> items)
    {
        var failures = new List<ValidationFailure>();

        foreach (var item in items)
        {
            if (item is { Quantity: < 4, Discount: > 0 })
            {
                failures.Add(new ValidationFailure(nameof(item.Quantity), "Discount is not allowed for purchases below 4 items."));
            }
            if (item.Quantity > 20)
            {
                failures.Add(new ValidationFailure(nameof(item.Quantity), "Cannot sell more than 20 identical items."));
            }
        }

        return failures;
    }
}