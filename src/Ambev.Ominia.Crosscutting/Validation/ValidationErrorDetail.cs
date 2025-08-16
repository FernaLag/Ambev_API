using Ambev.Ominia.Domain.Common.Validations;
using Ambev.Ominia.Domain.Interfaces;

namespace Ambev.Ominia.Crosscutting.Validation;

/// <summary>
/// Represents a validation error detail.
/// </summary>
public class ValidationErrorDetail : IValidationErrorDetail
{
    public string Error { get; init; } = string.Empty;
    public string Detail { get; init; } = string.Empty;

    public static explicit operator ValidationErrorDetail(ValidationFailure validationFailure) =>
        new()
        {
            Detail = validationFailure.ErrorMessage,
            Error = validationFailure.PropertyName
        };
}