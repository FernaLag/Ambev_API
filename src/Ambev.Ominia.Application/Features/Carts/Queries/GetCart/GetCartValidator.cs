namespace Ambev.Ominia.Application.Features.Carts.Queries.GetCart;

/// <summary>
/// Validator for GetCartCommand.
/// </summary>
public class GetCartValidator : AbstractValidator<GetCartQuery>
{
    public GetCartValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Cart ID must be greater than zero.");
    }
}