using FluentValidation.Results;

namespace Ambev.Ominia.Crosscutting.Validation;

public class ValidationResultDetail
{
    public bool IsValid { get; set; }
    public IEnumerable<ValidationErrorDetail> Errors { get; set; } = [];

    public ValidationResultDetail()
    {

    }

    public ValidationResultDetail(ValidationResult validationResult)
    {
        IsValid = validationResult.IsValid;
        Errors = validationResult.Errors.Select(error => (ValidationErrorDetail)(object)error);
    }
}
