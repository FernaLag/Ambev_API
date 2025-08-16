namespace Ambev.Ominia.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Validator for CreateProductCommand that defines validation rules for product creation.
/// </summary>
/// <remarks>
/// Validation rules include:
/// - Title: Required, must be between 3 and 100 characters.
/// - Price: Must be greater than 0.
/// - Description: Required.
/// - Category: Required.
/// - Image: Must be a valid URL.
/// - Rating Rate: Must be between 0 and 5.
/// - Rating Count: Must be greater than or equal to 0.
/// </remarks>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateProductCommandValidator with defined validation rules.
    /// </summary>
    public CreateProductCommandValidator()
    {
        RuleFor(product => product.Title)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(product => product.Price)
            .GreaterThan(0);

        RuleFor(product => product.Description)
            .NotEmpty();

        RuleFor(product => product.Category)
            .NotEmpty();

        //RuleFor(product => product.Image)
        //    .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
        //    .WithMessage("Invalid URL format.");

        RuleFor(product => product.Rating.Rate)
            .InclusiveBetween(0, 5);

        RuleFor(product => product.Rating.Count)
            .GreaterThanOrEqualTo(0);
    }
}