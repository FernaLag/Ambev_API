namespace Ambev.Ominia.Domain.ValueObjects;

/// <summary>
/// Represents the rating of a product, including the rating value and the number of votes.
/// This value object is used in the <see cref="Product"/> entity.
/// </summary>
/// <param name="Rate">The rating score (0 to 5 scale).</param>
/// <param name="Count">The total number of users who rated the product.</param>
public record Rating(double Rate, int Count);