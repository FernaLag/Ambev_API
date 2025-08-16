using Ambev.Ominia.Domain.Interfaces;

namespace Ambev.Ominia.Domain.Responses;

public abstract class GenericResponse<T>
{
    public T? Data { get; set; } = default;

    public bool Success { get; set; } = true;

    public string Message { get; set; } = string.Empty;

    public IEnumerable<IValidationErrorDetail>? Errors { get; set; } = null;
}