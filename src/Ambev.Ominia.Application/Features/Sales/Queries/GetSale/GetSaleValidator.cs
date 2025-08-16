namespace Ambev.Ominia.Application.Features.Sales.Queries.GetSale;

public class GetSaleValidator : AbstractValidator<GetSaleQuery>
{
    public GetSaleValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Sale ID must be greater than zero.");
    }
}