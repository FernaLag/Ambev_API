namespace Ambev.Ominia.Application.Features.Carts.Commands.CreateCart;

/// <summary>
/// Validator for CreateCartCommand that defines validation rules for cart creation.
/// </summary>
public class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
{
    public CreateCartCommandValidator()
    {
        RuleFor(cart => cart.UserId)
            .GreaterThan(0);

        RuleFor(cart => cart.Products)
            .NotEmpty();

        RuleForEach(cart => cart.Products)
            .SetValidator(new CreateCartItemCommandValidator());
    }
}

/// <summary>
/// Validator for individual cart items.
/// </summary>
public class CreateCartItemCommandValidator : AbstractValidator<CreateCartItemCommand>
{
    public CreateCartItemCommandValidator()
    {
        RuleFor(item => item.ProductId)
            .GreaterThan(0);

        RuleFor(item => item.Quantity)
            .GreaterThan(0);
    }
}