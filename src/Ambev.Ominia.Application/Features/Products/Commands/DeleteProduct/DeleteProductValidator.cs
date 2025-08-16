namespace Ambev.Ominia.Application.Features.Products.Commands.DeleteProduct;

/// <summary>
/// Validator for DeleteProductCommand.
/// </summary>
public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
{
    /// <summary>
    /// Initializes validation rules for DeleteProductCommand.
    /// </summary>
    public DeleteProductValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than zero.");
    }
}