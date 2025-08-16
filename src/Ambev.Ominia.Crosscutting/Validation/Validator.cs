using FluentValidation;

namespace Ambev.Ominia.Crosscutting.Validation;

public static class Validator
{
    public static async Task<IEnumerable<ValidationErrorDetail>> ValidateAsync<T>(T instance)
    {
        Type validatorType = typeof(IValidator<>).MakeGenericType(typeof(T));

        if (Activator.CreateInstance(validatorType) is not IValidator validator)
        {
            throw new InvalidOperationException($"No validator found for: {typeof(T).Name}");
        }

        var result = await validator.ValidateAsync(new ValidationContext<T>(instance));

        return !result.IsValid ? result.Errors.Select(error => (ValidationErrorDetail)(object)error) : [];
    }
}