using Ambev.Ominia.Application.Features.Sales.Commands.UpdateSale;
using Ambev.Ominia.Domain.Entities.Sales;
using Bogus;

namespace Ambev.Ominia.Application.Tests.Features.Sales.Builders;

public class UpdateSaleCommandBuilder
{
    private int _id = 1;
    private string _saleNumber = string.Empty;
    private DateTime _date = DateTime.UtcNow;
    private string _customer = string.Empty;
    private string _branch = string.Empty;
    private List<SaleItem>? _items = [];
    private readonly Faker _faker = new();

    public UpdateSaleCommandBuilder()
    {
        WithDefaults();
    }

    private UpdateSaleCommandBuilder WithDefaults()
    {
        _id = _faker.Random.Int(1, 1000);
        _saleNumber = _faker.Commerce.Ean13();
        _date = _faker.Date.Recent();
        _customer = _faker.Person.FullName;
        _branch = _faker.Company.CompanyName();
        _items = [new SaleItemBuilder().Build()];
        return this;
    }

    public UpdateSaleCommandBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public UpdateSaleCommandBuilder WithSaleNumber(string saleNumber)
    {
        _saleNumber = saleNumber;
        return this;
    }

    public UpdateSaleCommandBuilder WithDate(DateTime date)
    {
        _date = date;
        return this;
    }

    public UpdateSaleCommandBuilder WithCustomer(string customer)
    {
        _customer = customer;
        return this;
    }

    public UpdateSaleCommandBuilder WithBranch(string branch)
    {
        _branch = branch;
        return this;
    }

    public UpdateSaleCommandBuilder WithItems(List<SaleItem>? items)
    {
        _items = items;
        return this;
    }

    public UpdateSaleCommandBuilder WithInvalidId()
    {
        _id = 0;
        return this;
    }

    public UpdateSaleCommand Build()
    {
        return new UpdateSaleCommand
        {
            Id = _id,
            SaleNumber = _saleNumber,
            Date = _date,
            Customer = _customer,
            Branch = _branch,
            Items = _items ?? []
        };
    }
}