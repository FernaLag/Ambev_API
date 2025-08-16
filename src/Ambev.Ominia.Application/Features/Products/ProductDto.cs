using Ambev.Ominia.Domain.ValueObjects;

namespace Ambev.Ominia.Application.Features.Products;

public record ProductDto(
    int Id,
    string Title,
    decimal Price,
    string Description,
    string Category,
    string Image,
    Rating Rating,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record ProductSummaryDto(
    int Id,
    string Title,
    decimal Price,
    string Category,
    Rating Rating
);