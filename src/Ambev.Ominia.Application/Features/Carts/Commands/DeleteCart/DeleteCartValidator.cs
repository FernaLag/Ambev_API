namespace Ambev.Ominia.Application.Features.Carts.Commands.DeleteCart;

/// <summary>
/// Validator for DeleteCartCommand.
/// </summary>
public class DeleteCartValidator : AbstractValidator<DeleteCartCommand>
{
    public DeleteCartValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Cart ID must be greater than zero.");
    }
}