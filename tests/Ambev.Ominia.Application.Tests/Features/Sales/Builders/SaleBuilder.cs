using Ambev.Ominia.Domain.Entities.Sales;
using Bogus;

namespace Ambev.Ominia.Application.Tests.Features.Sales.Builders;

public class SaleBuilder
{
    private string _saleNumber = string.Empty;
    private DateTime _date = DateTime.UtcNow;
    private string _customer = string.Empty;
    private string _branch = string.Empty;
    private List<SaleItem> _items = [];
    private readonly Faker _faker = new();

    public SaleBuilder()
    {
        WithDefaults();
    }

    public SaleBuilder WithDefaults()
    {
        _saleNumber = _faker.Commerce.Ean13();
        _date = _faker.Date.Recent();
        _customer = _faker.Person.FullName;
        _branch = _faker.Company.CompanyName();
        _items = [new SaleItemBuilder().Build()];
        return this;
    }

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

    public SaleBuilder WithItem(SaleItem item)
    {
        _items = [item];
        return this;
    }

    public SaleBuilder WithMultipleItems(int count)
    {
        _items = [];
        for (int i = 0; i < count; i++)
        {
            _items.Add(new SaleItemBuilder().WithProductId(i + 1).Build());
        }
        return this;
    }

    public SaleBuilder WithItemsForDiscountTest(int quantity)
    {
        var item = new SaleItemBuilder()
            .WithQuantity(quantity)
            .WithUnitPrice(100m)
            .WithDiscount(0m)
            .Build();
        _items = [item];
        return this;
    }

    public Sale Build()
    {
        return new Sale(_saleNumber, _date, _customer, _branch, _items);
    }
}