namespace Ambev.Ominia.Application.Features.Products.Commands.UpdateProduct;

/// <summary>
/// Validator for UpdateProductCommand.
/// </summary>
/// <remarks>
/// This validator ensures that all product update fields meet
/// the required constraints, similar to the product creation process.
/// </remarks>
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateProductCommandValidator.
    /// </summary>
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than zero.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.Category)
            .NotEmpty();

        //RuleFor(x => x.Image)
        //    .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
        //    .WithMessage("Invalid URL format.");

        RuleFor(product => product.Rating.Rate)
            .InclusiveBetween(0, 5).WithMessage("Product rating must be between 0 and 5.");

        RuleFor(product => product.Rating.Count)
            .GreaterThanOrEqualTo(0).WithMessage("Product rating count must be zero or greater.");
    }
}