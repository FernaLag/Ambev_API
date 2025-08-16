using Ambev.Ominia.Application.Features.Sales.Commands.CreateSale;
using Ambev.Ominia.Domain.Entities.Sales;
using Bogus;

namespace Ambev.Ominia.Application.Tests.Features.Sales.Builders;

public class CreateSaleCommandBuilder
{
    private string _saleNumber = string.Empty;
    private DateTime _date = DateTime.UtcNow;
    private string _customer = string.Empty;
    private string _branch = string.Empty;
    private List<SaleItem> _items = [];
    private readonly Faker _faker = new();

    public CreateSaleCommandBuilder()
    {
        WithDefaults();
    }

    public CreateSaleCommandBuilder WithDefaults()
    {
        _saleNumber = _faker.Commerce.Ean13();
        _date = _faker.Date.Recent();
        _customer = _faker.Person.FullName;
        _branch = _faker.Company.CompanyName();
        _items = [new SaleItemBuilder().Build()];
        return this;
    }

    public CreateSaleCommandBuilder WithSaleNumber(string saleNumber)
    {
        _saleNumber = saleNumber;
        return this;
    }

    public CreateSaleCommandBuilder WithDate(DateTime date)
    {
        _date = date;
        return this;
    }

    public CreateSaleCommandBuilder WithCustomer(string customer)
    {
        _customer = customer;
        return this;
    }

    public CreateSaleCommandBuilder WithBranch(string branch)
    {
        _branch = branch;
        return this;
    }

    public CreateSaleCommandBuilder WithItems(List<SaleItem> items)
    {
        _items = items;
        return this;
    }

    public CreateSaleCommandBuilder WithEmptyItems()
    {
        _items = [];
        return this;
    }

    public CreateSaleCommandBuilder WithEmptySaleNumber()
    {
        _saleNumber = string.Empty;
        return this;
    }

    public CreateSaleCommandBuilder WithEmptyCustomer()
    {
        _customer = string.Empty;
        return this;
    }

    public CreateSaleCommandBuilder WithEmptyBranch()
    {
        _branch = string.Empty;
        return this;
    }

    public CreateSaleCommand Build()
    {
        return new CreateSaleCommand
        {
            SaleNumber = _saleNumber,
            Date = _date,
            Customer = _customer,
            Branch = _branch,
            Items = _items
        };
    }
}