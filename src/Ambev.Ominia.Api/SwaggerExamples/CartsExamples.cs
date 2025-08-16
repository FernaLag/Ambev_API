using Swashbuckle.AspNetCore.Filters;
using Ambev.Ominia.Application.Features.Carts.Commands.CreateCart;
using Ambev.Ominia.Application.Features.Carts.Commands.UpdateCart;
using Ambev.Ominia.Application.Features.Carts.Queries.ListCarts;
using Ambev.Ominia.Domain.Entities.Cart;

namespace Ambev.Ominia.Api.SwaggerExamples;

public class ListCartsQueryExample : IExamplesProvider<ListCartsQuery>
{
    public ListCartsQuery GetExamples()
    {
        return new ListCartsQuery
        {
            Page = 1,
            PageSize = 10,
            OrderBy = "Date"
        };
    }
}


public class CreateCartCommandExample : IExamplesProvider<CreateCartCommand>
{
    public CreateCartCommand GetExamples()
    {
        return new CreateCartCommand
        {
            UserId = 1,
            Date = DateTime.UtcNow,
            Products =
                [
                new CreateCartItemCommand
                    {
                    ProductId = 1,
                    Quantity = 2
                    },

                new CreateCartItemCommand
                    {
                    ProductId = 5,
                    Quantity = 1
                    }
                ]
            };
    }
}

public class UpdateCartCommandExample : IExamplesProvider<UpdateCartCommand>
{
    public UpdateCartCommand GetExamples()
    {
        return new UpdateCartCommand
        {
            Id = 1,
            UserId = 1,
            Date = DateTime.UtcNow,
            CartItems =
                [
                new CartItem
                    {
                    Id = 1,
                    CartId = 1,
                    ProductId = 1,
                    Quantity = 3
                    },

                new CartItem
                    {
                    Id = 2,
                    CartId = 1,
                    ProductId = 3,
                    Quantity = 1
                    }
                ]
            };
    }
}