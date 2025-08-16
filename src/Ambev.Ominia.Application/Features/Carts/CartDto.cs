namespace Ambev.Ominia.Application.Features.Carts;

public record CartDto(
    int Id,
    int UserId,
    DateTime Date,
    IReadOnlyList<CartItemDto> CartItems
);

public record CartItemDto(
    int Id,
    int ProductId,
    string ProductTitle,
    decimal ProductPrice,
    string ProductImage,
    int Quantity
);

public record CartSummaryDto(
    int Id,
    int UserId,
    DateTime Date
);