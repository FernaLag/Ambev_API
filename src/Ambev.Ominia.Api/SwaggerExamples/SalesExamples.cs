using Swashbuckle.AspNetCore.Filters;
using Ambev.Ominia.Application.Features.Sales.Commands.CreateSale;
using Ambev.Ominia.Application.Features.Sales.Commands.UpdateSale;
using Ambev.Ominia.Application.Features.Sales.Queries.ListSales;
using Ambev.Ominia.Domain.Entities.Sales;

namespace Ambev.Ominia.Api.SwaggerExamples;

public class ListSalesQueryExample : IExamplesProvider<ListSalesQuery>
{
    public ListSalesQuery GetExamples()
    {
        return new ListSalesQuery
        {
            Page = 1,
            PageSize = 10,
            OrderBy = "Date"
        };
    }
}

public class CreateSaleCommandExample : IExamplesProvider<CreateSaleCommand>
{
    public CreateSaleCommand GetExamples()
    {
        return new CreateSaleCommand
        {
            SaleNumber = "SALE-2030-031",
            Date = DateTime.UtcNow,
            Customer = "João Silva",
            Branch = "Filial Centro",
            Items =
                [
                new SaleItem(0, 1, 2, 109.95m, 0m)
                    {
                    Product = null
                    },

                new SaleItem(0, 5, 1, 695.00m, 50.00m)
                    {
                    Product = null
                    }
                ]
            };
    }
}

public class UpdateSaleCommandExample : IExamplesProvider<UpdateSaleCommand>
{
    public UpdateSaleCommand GetExamples()
    {
        return new UpdateSaleCommand
        {
            Id = 31,
            SaleNumber = "ULTIMATE UPDATED SALE-2030-031",
            Date = DateTime.UtcNow,
            Customer = "Grande João Silva Santos",
            Branch = "Filial Norte",
            Items =
                [
                new SaleItem(31, 1, 3, 109.95m, 10.00m, 74)
                    {
                    Product = null
                    },

                new SaleItem(31, 3, 1, 55.99m, 0m, 75)
                    {
                    Product = null
                    }
                ]
            };
    }
}