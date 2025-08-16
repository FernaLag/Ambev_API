namespace Ambev.Ominia.Application.Features.Sales.Commands.DeleteSale;

public class DeleteSaleValidator : AbstractValidator<DeleteSaleCommand>
{
    public DeleteSaleValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Sale ID must be greater than zero.");
    }
}