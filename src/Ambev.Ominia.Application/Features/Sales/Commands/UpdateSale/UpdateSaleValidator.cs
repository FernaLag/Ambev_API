using Ambev.Ominia.Domain.Entities.Sales;

namespace Ambev.Ominia.Application.Features.Sales.Commands.UpdateSale;

public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
    public UpdateSaleCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Sale ID must be greater than zero.");

        RuleFor(x => x.SaleNumber)
            .NotEmpty()
            .WithMessage("Sale number must not be empty.");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date must not be empty.");

        RuleFor(x => x.Customer)
            .NotEmpty()
            .WithMessage("Customer name must not be empty.");

        RuleFor(x => x.Branch)
            .NotEmpty()
            .WithMessage("Branch must not be empty.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("A sale must contain at least one item.");

        RuleForEach(x => x.Items).SetValidator(new UpdateSaleItemCommandValidator());
    }
}

public class UpdateSaleItemCommandValidator : AbstractValidator<SaleItem>
{
    public UpdateSaleItemCommandValidator()
    {
        RuleFor(item => item.SaleId).GreaterThan(0);
        RuleFor(item => item.ProductId).GreaterThan(0);
        RuleFor(item => item.Quantity).GreaterThan(0);
        RuleFor(item => item.UnitPrice).GreaterThan(0);
        RuleFor(item => item.Discount).GreaterThanOrEqualTo(0);
    }
}