namespace Ambev.Ominia.Domain.Common.Validations;

    public static partial class ValidationExtensions
    {
        public static List<ValidationFailure> Null<TList>(
            this List<ValidationFailure> source,
            IEnumerable<TList> value,
            string propertyName)
            => Null(
                source,
                value,
                propertyName,
                $"O campo '{propertyName}' deve ser nulo.");

        public static List<ValidationFailure> Null<TList>(
            this List<ValidationFailure> source,
            IEnumerable<TList> value,
            string propertyName,
            string message)
        {
            if (value is null) { return source; }

            return source.Add(propertyName, message);
        }

        public static List<ValidationFailure> NotNull<TList>(
            this List<ValidationFailure> source,
            IEnumerable<TList> value,
            string propertyName)
            => NotNull(
                source,
                value,
                propertyName,
                $"O campo '{propertyName}' não deve ser nulo.");

        public static List<ValidationFailure> NotNull<TList>(
            this List<ValidationFailure> source,
            IEnumerable<TList> value,
            string propertyName,
            string message)
        {
            if (value is not null) { return source; }

            return source.Add(propertyName, message);
        }

        public static List<ValidationFailure> Empty<TList>(
            this List<ValidationFailure> source,
            IEnumerable<TList> value,
            string propertyName)
            => Empty(
                source,
                value,
                propertyName,
                $"A lista '{propertyName}' deve estar vazia.");

        public static List<ValidationFailure> Empty<TList>(
            this List<ValidationFailure> source,
            IEnumerable<TList> value,
            string propertyName,
            string message)
        {
            if (value is null) { return source; }
            if (!value.Any()) { return source; }

            return source.Add(propertyName, message);
        }

        public static List<ValidationFailure> NotEmpty<TList>(
            this List<ValidationFailure> source,
            IEnumerable<TList> value,
            string propertyName)
            => NotEmpty(
                source,
                value,
                propertyName,
                $"A lista '{propertyName}' não deve estar vazia.");

        public static List<ValidationFailure> NotEmpty<TList>(
            this List<ValidationFailure> source,
            IEnumerable<TList> value,
            string propertyName,
            string message)
        {
            if (value is not null && value.Any()) { return source; }

            return source.Add(propertyName, message);
        }

        public static List<ValidationFailure> MinimumLength<TList>(
            this List<ValidationFailure> source,
            IEnumerable<TList> value,
            int minimumLength,
            string propertyName)
            => MinimumLength(
                source,
                value,
                minimumLength,
                propertyName,
                $"A lista '{propertyName}' deve conter mais ou igual a {minimumLength} elementos.");

        public static List<ValidationFailure> MinimumLength<TList>(
            this List<ValidationFailure> source,
            IEnumerable<TList> value,
            int minimumLength,
            string propertyName,
            string message)
        {
            if (value?.Count() >= minimumLength) { return source; }

            return source.Add(propertyName, message);
        }

        public static List<ValidationFailure> MaximumLength<TList>(
            this List<ValidationFailure> source,
            IEnumerable<TList> value,
            int maximumLength,
            string propertyName)
            => MaximumLength(
                source,
                value,
                maximumLength,
                propertyName,
                $"A lista '{propertyName}' deve conter {maximumLength} ou menos.");

        public static List<ValidationFailure> MaximumLength<TList>(
            this List<ValidationFailure> source,
            IEnumerable<TList> value,
            int maximumLength,
            string propertyName,
            string message)
        {
            if (value?.Count() <= maximumLength) { return source; }

            return source.Add(propertyName, message);
        }

        public static List<ValidationFailure> ContainsAny<TList>(
            this List<ValidationFailure> source,
            IEnumerable<TList> value,
            Func<TList, bool> predicate,
            string propertyName,
            string message)
        {
            if (value.Any(predicate)) { return source; }

            return source.Add(propertyName, message);
        }

        public static List<ValidationFailure> NotContainsAny<TList>(
            this List<ValidationFailure> source,
            IEnumerable<TList> value,
            Func<TList, bool> predicate,
            string propertyName,
            string message)
        {
            if (!value.Any(predicate)) { return source; }

            return source.Add(propertyName, message);
        }

        public static List<ValidationFailure> AllAre<TList>(
            this List<ValidationFailure> source,
            IEnumerable<TList> value,
            Func<TList, bool> predicate,
            string propertyName,
            string message)
        {
            if (value is not null && value.All(predicate)) { return source; }

            return source.Add(propertyName, message);
        }

        public static List<ValidationFailure> OnlyUnique<TSource, TKey>(
            this List<ValidationFailure> source,
            IEnumerable<TSource> value,
            Func<TSource, TKey> keySelector,
            string propertyName,
            string message)
        {
            if (!value.GroupBy(keySelector).Any(g => g.Count() > 1)) { return source; }

            return source.Add(propertyName, message);
        }
    }