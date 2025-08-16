using Ambev.Ominia.Domain.Extensions;

namespace Ambev.Ominia.Domain.Common.Validations;

public static partial class ValidationExtensions
    {
    public static List<ValidationFailure> Null(
        this List<ValidationFailure> source,
        string value,
        string propertyName)
        {
        return Null(
            source,
            value,
            propertyName,
            $"Campo '{propertyName}' não deve ser informado.");
        }

    public static List<ValidationFailure> Null(
        this List<ValidationFailure> source,
        string value,
        string propertyName,
        string message)
        {
        if (value is null) { return source; }

        return source.Add(propertyName, message);
        }

    public static List<ValidationFailure> NotNull(
        this List<ValidationFailure> source,
        string value,
        string propertyName)
        {
        return NotNull(
            source,
            value,
            propertyName,
            $"Campo '{propertyName}' não informado.");
        }

    public static List<ValidationFailure> NotNull(
        this List<ValidationFailure> source,
        string value,
        string propertyName,
        string message)
        {
        if (value is not null) { return source; }

        return source.Add(propertyName, message);
        }

    public static List<ValidationFailure> NullOrEmpty(
        this List<ValidationFailure> source,
        string value,
        string propertyName)
        {
        return NullOrEmpty(
            source,
            value,
            propertyName,
            $"Campo '{propertyName}' não deve ser informado.");
        }

    public static List<ValidationFailure> NullOrEmpty(
        this List<ValidationFailure> source,
        string value,
        string propertyName,
        string message)
        {
        if (string.IsNullOrEmpty(value)) { return source; }

        return source.Add(propertyName, message);
        }

    public static List<ValidationFailure> NotNullOrEmpty(
        this List<ValidationFailure> source,
        string value,
        string propertyName)
        {
        return NotNullOrEmpty(
            source,
            value,
            propertyName,
            $"Campo '{propertyName}' deve ser informado.");
        }

    public static List<ValidationFailure> NotNullOrEmpty(
        this List<ValidationFailure> source,
        string value,
        string propertyName,
        string message)
        {
        if (!string.IsNullOrEmpty(value)) { return source; }

        return source.Add(propertyName, message);
        }

    public static List<ValidationFailure> NullOrWhiteSpace(
        this List<ValidationFailure> source,
        string value,
        string propertyName)
        {
        return NullOrWhiteSpace(
            source,
            value,
            propertyName,
            $"Campo '{propertyName}' não deve ser informado.");
        }

    public static List<ValidationFailure> NullOrWhiteSpace(
        this List<ValidationFailure> source,
        string value,
        string propertyName,
        string message)
        {
        if (string.IsNullOrWhiteSpace(value)) { return source; }

        return source.Add(propertyName, message);
        }

    public static List<ValidationFailure> NotNullOrWhiteSpace(
        this List<ValidationFailure> source,
        string value,
        string propertyName)
        {
        return NotNullOrWhiteSpace(
            source,
            value,
            propertyName,
            $"O campo '{propertyName}' não foi informado.");
        }

    public static List<ValidationFailure> NotNullOrWhiteSpace(
        this List<ValidationFailure> source,
        string value,
        string propertyName,
        string message)
        {
        if (!string.IsNullOrWhiteSpace(value)) { return source; }

        return source.Add(propertyName, message);
        }

    public static List<ValidationFailure> Equals(
        this List<ValidationFailure> source,
        string value,
        string comparer,
        string propertyName)
        {
        return Equals(
            source,
            value,
            comparer,
            propertyName,
            $"Campo '{propertyName}' tem valor '{value}' diferente de '{comparer}'.");
        }

    public static List<ValidationFailure> Equals(
        this List<ValidationFailure> source,
        string value,
        string comparer,
        string propertyName,
        string message)
        {
        if (value == comparer) { return source; }

        return source.Add(propertyName, message);
        }

    public static List<ValidationFailure> NotEquals(
        this List<ValidationFailure> source,
        string value,
        string comparer,
        string propertyName)
        {
        return NotEquals(
            source,
            value,
            comparer,
            propertyName,
            $"Campo '{propertyName}' não deve ter valor '{value}'.");
        }

    public static List<ValidationFailure> NotEquals(
        this List<ValidationFailure> source,
        string value,
        string comparer,
        string propertyName,
        string message)
        {
        if (value != comparer) { return source; }

        return source.Add(propertyName, message);
        }

    public static List<ValidationFailure> LengthEquals(
        this List<ValidationFailure> source,
        string value,
        int expectedLength,
        string propertyName)
        {
        return LengthEquals(
            source,
            value,
            expectedLength,
            propertyName,
            $"Campo '{propertyName}' deve ser de tamanho '{expectedLength}'");
        }

    public static List<ValidationFailure> LengthEquals(
        this List<ValidationFailure> source,
        string value,
        int expectedLength,
        string propertyName,
        string message)
        {
        if (value is null) { return source; }
        if (value.Length == expectedLength) { return source; }

        return source.Add(propertyName, message);
        }

    public static List<ValidationFailure> LengthNotEquals(
        this List<ValidationFailure> source,
        string value,
        int length,
        string propertyName) =>
        LengthNotEquals(
            source,
            value,
            length,
            propertyName,
            $"O campo '{propertyName}' deve ter tamanho diferente de '{length}'");

    public static List<ValidationFailure> LengthNotEquals(
        this List<ValidationFailure> source,
        string value,
        int length,
        string propertyName,
        string message)
        {
        if (value is null) { return source; }
        if (value.Length != length) { return source; }

        return source.Add(propertyName, message);
        }

    public static List<ValidationFailure> Contains(
        this List<ValidationFailure> source,
        string value,
        string comparer,
        string propertyName)
        {
        return Contains(
            source,
            value,
            comparer,
            StringComparison.Ordinal,
            propertyName);
        }

    public static List<ValidationFailure> Contains(
        this List<ValidationFailure> source,
        string value,
        string comparer,
        StringComparison comparisonType,
        string propertyName)
        {
        return Contains(
            source,
            value,
            comparer,
            comparisonType,
            propertyName,
            $"Campo '{propertyName}' deve conter '{comparer}'");
        }

    public static List<ValidationFailure> Contains(
        this List<ValidationFailure> source,
        string value,
        string comparer,
        StringComparison comparisonType,
        string propertyName,
        string message)
        {
        value ??= string.Empty;
        if (value.Contains(comparer, comparisonType)) { return source; }

        return source.Add(propertyName, message);
        }

    public static List<ValidationFailure> NotContains(
        this List<ValidationFailure> source,
        string value,
        string comparer,
        string propertyName)
        {
        return NotContains(
            source,
            value,
            comparer,
            StringComparison.Ordinal,
            propertyName);
        }

    public static List<ValidationFailure> NotContains(
        this List<ValidationFailure> source,
        string value,
        string comparer,
        StringComparison comparisonType,
        string propertyName)
        {
        return NotContains(
            source,
            value,
            comparer,
            comparisonType,
            propertyName,
            $"Campo '{propertyName}' não deve conter '{comparer}'");
        }

    public static List<ValidationFailure> NotContains(
        this List<ValidationFailure> source,
        string value,
        string comparer,
        StringComparison comparisonType,
        string propertyName,
        string message)
        {
        value ??= string.Empty;
        if (!value.Contains(comparer, comparisonType)) { return source; }

        return source.Add(propertyName, message);
        }

    public static List<ValidationFailure> MinimumLength(
        this List<ValidationFailure> source,
        string value,
        int minimumLength,
        string propertyName)
        {
        return MinimumLength(
            source,
            value,
            minimumLength,
            propertyName,
            $"O campo '{propertyName}' deve conter ao menos '{minimumLength}' caracteres.");
        }

    public static List<ValidationFailure> MinimumLength(
        this List<ValidationFailure> source,
        string value,
        int minimumLength,
        string propertyName,
        string message)
        {
        if (value is null) { return source; }
        if (value.Length >= minimumLength) { return source; }

        return source.Add(propertyName, message);
        }

    public static List<ValidationFailure> MaximumLength(
        this List<ValidationFailure> source,
        string value,
        int maximumLength,
        string propertyName)
        {
        return MaximumLength(
            source,
            value,
            maximumLength,
            propertyName,
            $"O campo '{propertyName}' deve conter no máximo '{maximumLength}' caracteres.");
        }

    public static List<ValidationFailure> MaximumLength(
        this List<ValidationFailure> source,
        string value,
        int maximumLength,
        string propertyName,
        string message)
        {
        if (value is null) { return source; }
        if (value.Length <= maximumLength) { return source; }

        return source.Add(propertyName, message);
        }

    public static List<ValidationFailure> ContainsAny(
        this List<ValidationFailure> source,
        string value,
        Func<char, bool> predicate,
        string propertyName,
        string message)
        {
        if (value is null) { return source; }
        if (value.Any(predicate)) { return source; }

        return source.Add(propertyName, message);
        }

    public static List<ValidationFailure> NotContainsAny(
        this List<ValidationFailure> source,
        string value,
        Func<char, bool> predicate,
        string propertyName,
        string message)
        {
        if (value is null) { return source; }
        if (!value.Any(predicate)) { return source; }

        return source.Add(propertyName, message);
        }

    public static List<ValidationFailure> Email(
        this List<ValidationFailure> source,
        string email,
        string propertyName)
        {
        if (email.IsValidEmail()) { return source; }

        return source.Add(propertyName, "E-mail inválido");
        }

    public static List<ValidationFailure> EmailOrNull(
        this List<ValidationFailure> source,
        string email,
        string propertyName)
        => email is null ? source : Email(source, email, propertyName);
    }