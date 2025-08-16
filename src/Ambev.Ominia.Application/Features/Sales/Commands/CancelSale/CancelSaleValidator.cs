namespace Ambev.Ominia.Application.Features.Sales.Commands.CancelSale;

public class CancelSaleValidator : AbstractValidator<CancelSaleCommand>
{
    public CancelSaleValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Sale ID must be greater than zero.");
    }
}