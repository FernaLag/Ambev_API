namespace Ambev.Ominia.Application.Features.Carts.Commands.UpdateCart;

/// <summary>
/// Validator for UpdateCartCommand.
/// </summary>
public class UpdateCartCommandValidator : AbstractValidator<UpdateCartCommand>
{
    public UpdateCartCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Cart ID must be greater than zero.");

        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.CartItems)
            .NotEmpty();
    }
}