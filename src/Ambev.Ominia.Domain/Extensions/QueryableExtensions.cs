using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Collections.Concurrent;

namespace Ambev.Ominia.Domain.Extensions;

/// <summary>
/// Extension methods for IQueryable to provide common sorting functionality.
/// Uses the robust implementation from SaleRepository with regex parsing and automatic field detection.
/// </summary>
public static class QueryableExtensions
{
    private static readonly ConcurrentDictionary<Type, HashSet<string>> _allowedFieldsCache = new();

    /// <summary>
    /// Applies sorting to a queryable based on the provided order string.
    /// Automatically detects allowed fields from the entity's public properties.
    /// Uses the same robust implementation as SaleRepository with regex parsing.
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    /// <param name="source">The queryable source</param>
    /// <param name="order">The sorting criteria (e.g., "field1 desc, field2 asc")</param>
    /// <param name="defaultOrderBy">Default ordering expression when no valid sort is provided</param>
    /// <returns>The sorted queryable</returns>
    public static IQueryable<T> ApplySorting<T>(
        this IQueryable<T> source, 
        string? order, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? defaultOrderBy = null) where T : class
    {
        var allowedSortFields = GetAllowedSortFields<T>();
        return ApplySorting(source, order, allowedSortFields, defaultOrderBy);
    }

    /// <summary>
    /// Applies sorting to a queryable based on the provided order string and explicit allowed fields.
    /// Uses the same robust implementation as SaleRepository with regex parsing.
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    /// <param name="source">The queryable source</param>
    /// <param name="order">The sorting criteria (e.g., "field1 desc, field2 asc")</param>
    /// <param name="allowedSortFields">Set of allowed field names for sorting</param>
    /// <param name="defaultOrderBy">Default ordering expression when no valid sort is provided</param>
    /// <returns>The sorted queryable</returns>
    public static IQueryable<T> ApplySorting<T>(
        this IQueryable<T> source, 
        string? order, 
        HashSet<string> allowedSortFields,
        Func<IQueryable<T>, IOrderedQueryable<T>>? defaultOrderBy = null) where T : class
    {
        if (string.IsNullOrWhiteSpace(order))
        {
            return defaultOrderBy?.Invoke(source) ?? source.OrderBy(CreatePropertyExpression<T>("Id"));
        }

        var parts = Regex.Matches(order, @"(?<field>[a-zA-Z_][a-zA-Z0-9_]*)\s*(?:,|\s+)?\s*(?<dir>asc|desc)?", RegexOptions.IgnoreCase)
            .Select(m => new {
                Field = m.Groups["field"].Value,
                Dir = m.Groups["dir"].Success ? m.Groups["dir"].Value : "asc"
            })
            .Where(x => allowedSortFields.Contains(x.Field))
            .ToList();

        if (parts.Count == 0)
        {
            return defaultOrderBy?.Invoke(source) ?? source.OrderBy(CreatePropertyExpression<T>("Id"));
        }

        IOrderedQueryable<T>? ordered = null;
        for (int i = 0; i < parts.Count; i++)
        {
            bool desc = parts[i].Dir.Equals("desc", StringComparison.OrdinalIgnoreCase);
            string field = parts[i].Field;
            var propertyExpression = CreatePropertyExpression<T>(field);

            ordered = i == 0
                ? (desc
                    ? source.OrderByDescending(propertyExpression)
                    : source.OrderBy(propertyExpression))
                : (desc
                    ? ordered!.ThenByDescending(propertyExpression)
                    : ordered!.ThenBy(propertyExpression));
        }

        return ordered!;
    }

    /// <summary>
    /// Gets the allowed sort fields for an entity type by examining its public properties.
    /// Results are cached for performance.
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    /// <returns>HashSet of allowed field names</returns>
    private static HashSet<string> GetAllowedSortFields<T>() where T : class
    {
        return _allowedFieldsCache.GetOrAdd(typeof(T), type =>
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && IsSimpleType(p.PropertyType))
                .Select(p => p.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            
            return properties;
        });
    }

    /// <summary>
    /// Determines if a type is a simple type that can be used for sorting.
    /// </summary>
    /// <param name="type">The type to check</param>
    /// <returns>True if the type is simple and sortable</returns>
    private static bool IsSimpleType(Type type)
    {
        // Handle nullable types
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
        
        return underlyingType.IsPrimitive ||
               underlyingType == typeof(string) ||
               underlyingType == typeof(DateTime) ||
               underlyingType == typeof(DateTimeOffset) ||
               underlyingType == typeof(TimeSpan) ||
               underlyingType == typeof(Guid) ||
               underlyingType == typeof(decimal) ||
               underlyingType.IsEnum;
    }

    /// <summary>
    /// Creates a property expression for dynamic sorting without EF dependency.
    /// </summary>
    private static Expression<Func<T, object>> CreatePropertyExpression<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var converted = Expression.Convert(property, typeof(object));
        return Expression.Lambda<Func<T, object>>(converted, parameter);
    }
}