namespace Ambev.Ominia.Domain.Common.Validations;

public static partial class ValidationExtensions
{
    public static List<ValidationFailure> Add(
        this List<ValidationFailure> source,
        string property,
        string message)
    {
        source.Add(new ValidationFailure(property, message));
        return source;
    }

    public static List<TOutput> Join<TInput, TOutput>(
        this IEnumerable<TInput> source,
        params IEnumerable<TOutput>[] validacoesList)
        where TInput : ValidationFailure
        where TOutput : ValidationFailure
    {
        var sourceList = source.ToList();

        foreach (var validacoes in validacoesList)
        {
            sourceList.AddRange(validacoes.Cast<TInput>());
        }

        return [.. sourceList.Cast<TOutput>()];
    }

    public static List<ValidationFailure> Join(
        this List<ValidationFailure> source,
        params List<ValidationFailure>[] validacoesList)
    {
        foreach (var validacoes in validacoesList)
        {
            source.AddRange(validacoes);
        }

        return source;
    }

    public static List<ValidationFailure> JoinWhen(
        this List<ValidationFailure> source,
        bool? shouldJoin,
        params List<ValidationFailure>[] validacoesList)
    {
        if (!shouldJoin.HasValue || !shouldJoin.Value) { return source; }
        return source.Join(validacoesList);
    }

    public static List<ValidationFailure> JoinWhen(
        this List<ValidationFailure> source,
        bool? shouldJoin,
        Func<List<ValidationFailure>, List<ValidationFailure>> predicate)
    {
        if (!shouldJoin.HasValue || !shouldJoin.Value) { return source; }
        return predicate.Invoke(source);
    }
}