using System.Runtime.CompilerServices;
using Ambev.Ominia.Domain.Exceptions;

namespace Ambev.Ominia.Domain.Common.Validations;

public static class Guard
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Enforce(List<ValidationFailure> failures)
    {
        if (failures is null) { return; }
        if (failures.Count is 0) { return; }
        throw new ValidationException(failures);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NotNull([ValidatedNotNull] object value, string propertyName)
    {
        Enforce(Validation.Init.NotNull(value, propertyName));
    }
}