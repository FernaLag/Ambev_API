namespace Ambev.Ominia.Domain.Common.Validations;

/// <summary>
/// Defines a validation failure.
/// </summary>
public record ValidationFailure(string PropertyName, string ErrorMessage)
{
    public override string ToString()
    {
        return PropertyName + " : " + ErrorMessage;
    }
}