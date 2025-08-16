namespace Ambev.Ominia.Domain.Interfaces;

/// <summary>
/// Defines the structure for validation error details.
/// </summary>
public interface IValidationErrorDetail
{
    /// <summary>
    /// Gets the error code.
    /// </summary>
    string Error { get; }

    /// <summary>
    /// Gets the detailed error message.
    /// </summary>
    string Detail { get; }
}