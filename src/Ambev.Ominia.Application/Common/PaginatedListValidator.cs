using Ambev.Ominia.Application.Features.Carts.Queries.ListCarts;
using Ambev.Ominia.Application.Features.Products.Queries.ListProducts;
using Ambev.Ominia.Application.Features.Sales.Queries.ListSales;

namespace Ambev.Ominia.Application.Common;

public class PaginatedListValidator<T> : AbstractValidator<T> where T : PaginatedListQuery
{
    protected PaginatedListValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than zero.");

        RuleFor(x => x.PageSize).GreaterThan(0).LessThanOrEqualTo(100).WithMessage("Size must be between 1 and 100.");

        RuleFor(x => x.OrderBy).Matches(@"^([a-zA-Z_]+ (asc|desc), )*([a-zA-Z_]+ (asc|desc))?$")
            .When(x => !string.IsNullOrEmpty(x.OrderBy))
            .WithMessage("Invalid format, use 'PropertyName asc/desc' Example: 'Title desc, Quantity asc'");
    }
}

public sealed class ListSalesValidator : PaginatedListValidator<ListSalesQuery> { }

public sealed class ListCartsValidator : PaginatedListValidator<ListCartsQuery> { }

public sealed class ListProductsValidator : PaginatedListValidator<ListProductsQuery> { }