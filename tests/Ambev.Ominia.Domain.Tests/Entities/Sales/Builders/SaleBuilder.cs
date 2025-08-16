using Ambev.Ominia.Domain.Entities.Sales;

namespace Ambev.Ominia.Domain.Tests.Entities.Sales.Builders;

public class SaleBuilder
{
    private string _saleNumber = "SALE-2024-001";
    private DateTime _date = DateTime.UtcNow;
    private string _customer = "Test Customer";
    private string _branch = "Test Branch";
    private List<SaleItem> _items = [];

    public static SaleBuilder New() => new();

    public SaleBuilder WithSaleNumber(string saleNumber)
    {
        _saleNumber = saleNumber;
        return this;
    }

    public SaleBuilder WithDate(DateTime date)
    {
        _date = date;
        return this;
    }

    public SaleBuilder WithCustomer(string customer)
    {
        _customer = customer;
        return this;
    }

    public SaleBuilder WithBranch(string branch)
    {
        _branch = branch;
        return this;
    }

    public SaleBuilder WithItems(List<SaleItem> items)
    {
        _items = items;
        return this;
    }

    public SaleBuilder WithEmptyItems()
    {
        _items = [];
        return this;
    }

    public SaleBuilder WithNullSaleNumber()
    {
        _saleNumber = null!;
        return this;
    }

    public SaleBuilder WithNullCustomer()
    {
        _customer = null!;
        return this;
    }

    public SaleBuilder WithNullBranch()
    {
        _branch = null!;
        return this;
    }

    public SaleBuilder WithValidItems()
    {
        _items =
            [
            SaleItemBuilder.New().WithQuantity(5).WithDiscount(1.00m).Build(),
            SaleItemBuilder.New().WithQuantity(10).WithDiscount(2.00m).Build()
            ];
        return this;
    }

    public Sale Build()
    {
        return new Sale(_saleNumber, _date, _customer, _branch, _items);
    }
}