using Ambev.Ominia.Application.Features.Sales.Commands.DeleteSale;
using Ambev.Ominia.Application.Features.Sales.Commands.CancelSale;
using Ambev.Ominia.Application.Features.Sales.Queries.GetSale;
using Ambev.Ominia.Application.Features.Sales.Queries.ListSales;
using Bogus;

namespace Ambev.Ominia.Application.Tests.Features.Sales.Builders;

public class DeleteSaleCommandBuilder
{
    private int _id = 1;
    private readonly Faker _faker = new();

    public DeleteSaleCommandBuilder()
    {
        WithDefaults();
    }

    public DeleteSaleCommandBuilder WithDefaults()
    {
        _id = _faker.Random.Int(1, 1000);
        return this;
    }

    public DeleteSaleCommandBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public DeleteSaleCommand Build()
    {
        return new DeleteSaleCommand(_id);
    }
}

public class CancelSaleCommandBuilder
{
    private int _id = 1;
    private readonly Faker _faker = new();

    public CancelSaleCommandBuilder()
    {
        WithDefaults();
    }

    public CancelSaleCommandBuilder WithDefaults()
    {
        _id = _faker.Random.Int(1, 1000);
        return this;
    }

    public CancelSaleCommandBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public CancelSaleCommand Build()
    {
        return new CancelSaleCommand(_id);
    }
}

public class GetSaleQueryBuilder
{
    private int _id = 1;
    private readonly Faker _faker = new();

    public GetSaleQueryBuilder()
    {
        WithDefaults();
    }

    public GetSaleQueryBuilder WithDefaults()
    {
        _id = _faker.Random.Int(1, 1000);
        return this;
    }

    public GetSaleQueryBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public GetSaleQuery Build()
    {
        return new GetSaleQuery(_id);
    }
}

public class ListSalesQueryBuilder
{
    private int _page = 1;
    private int _pageSize = 10;
    private string _orderBy = "Date";
    private readonly Faker _faker = new();

    public ListSalesQueryBuilder()
    {
        WithDefaults();
    }

    public ListSalesQueryBuilder WithDefaults()
    {
        _page = _faker.Random.Int(1, 10);
        _pageSize = _faker.Random.Int(5, 50);
        _orderBy = _faker.PickRandom("Date", "Customer", "Branch", "TotalAmount");
        return this;
    }

    public ListSalesQueryBuilder WithPage(int page)
    {
        _page = page;
        return this;
    }

    public ListSalesQueryBuilder WithPageSize(int pageSize)
    {
        _pageSize = pageSize;
        return this;
    }

    public ListSalesQueryBuilder WithOrderBy(string orderBy)
    {
        _orderBy = orderBy;
        return this;
    }

    public ListSalesQuery Build()
    {
        return new ListSalesQuery
        {
            Page = _page,
            PageSize = _pageSize,
            OrderBy = _orderBy
        };
    }
}