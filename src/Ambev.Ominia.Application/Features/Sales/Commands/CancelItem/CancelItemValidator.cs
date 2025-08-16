namespace Ambev.Ominia.Application.Features.Sales.Commands.CancelItem;

/// <summary>
/// Validator for CancelItemCommand.
/// </summary>
public class CancelItemValidator : AbstractValidator<CancelItemCommand>
{
    /// <summary>
    /// Initializes validation rules for CancelItemCommand.
    /// </summary>
    public CancelItemValidator()
    {
        RuleFor(x => x.SaleId)
            .GreaterThan(0)
            .WithMessage("Sale ID must be greater than zero.");

        RuleFor(x => x.ItemId)
            .GreaterThan(0)
            .WithMessage("Item ID must be greater than zero.");
    }
}