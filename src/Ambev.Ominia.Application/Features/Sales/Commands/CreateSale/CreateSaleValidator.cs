using Ambev.Ominia.Domain.Entities.Sales;

namespace Ambev.Ominia.Application.Features.Sales.Commands.CreateSale;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty();

        RuleFor(sale => sale.Customer)
            .NotEmpty();

        RuleFor(sale => sale.Branch)
            .NotEmpty();

        RuleFor(sale => sale.Items)
            .NotEmpty();

        RuleForEach(sale => sale.Items).SetValidator(new CreateSaleItemCommandValidator());
    }
}

public class CreateSaleItemCommandValidator : AbstractValidator<SaleItem>
{
    public CreateSaleItemCommandValidator()
    {
        RuleFor(item => item.ProductId).GreaterThan(0);
        RuleFor(item => item.Quantity).GreaterThan(0);
        RuleFor(item => item.UnitPrice).GreaterThan(0);
        RuleFor(item => item.Discount).GreaterThanOrEqualTo(0);
    }
}