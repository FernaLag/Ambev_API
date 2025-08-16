namespace Ambev.Ominia.Domain.Common.Validations;

public static partial class ValidationExtensions
{
    public static List<ValidationFailure> Null(
        this List<ValidationFailure> source,
        object value,
        string propertyName)
        => Null(
            source,
            value,
            propertyName,
            $"O campo '{propertyName}' não deve ser informado.");

    public static List<ValidationFailure> Null(
        this List<ValidationFailure> source,
        object value,
        string propertyName,
        string message)
    {
        if (value is null) { return source; }

        return source.Add(propertyName, message);
    }

    public static List<ValidationFailure> NotNull(
        this List<ValidationFailure> source,
        object value,
        string propertyName)
        => NotNull(
            source,
            value,
            propertyName,
            $"O campo '{propertyName}' não foi informado.");

    public static List<ValidationFailure> NotNull(
        this List<ValidationFailure> source,
        object value,
        string propertyName,
        string message)
    {
        if (value is not null) { return source; }
        return source.Add(propertyName, message);
    }

    public static List<ValidationFailure> Equals(
        this List<ValidationFailure> source,
        object value,
        object comparer,
        string propertyName)
        => Equals(
            source,
            value,
            comparer,
            propertyName,
            $"O campo '{propertyName}' tem valor diferente do esperado.");

    public static List<ValidationFailure> Equals(
        this List<ValidationFailure> source,
        object value,
        object comparer,
        string propertyName,
        string message)
    {
        if (value is null || comparer is null) { return source; }
        if (value.Equals(comparer)) { return source; }

        return source.Add(propertyName, message);
    }

    public static List<ValidationFailure> NotEquals(
        this List<ValidationFailure> source,
        object value,
        object comparer,
        string propertyName)
        => NotEquals(
            source,
            value,
            comparer,
            propertyName,
            $"O campo '{propertyName}' deve ter valor diferente ao que foi comparado.");

    public static List<ValidationFailure> NotEquals(
        this List<ValidationFailure> source,
        object value,
        object comparer,
        string propertyName,
        string message)
    {
        if (value is null || comparer is null) { return source; }
        if (!value.Equals(comparer)) { return source; }

        return source.Add(propertyName, message);
    }
}