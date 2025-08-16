using AutoMapper;
using Ambev.Ominia.Application.Features.Sales;
using Ambev.Ominia.Application.Features.Sales.Commands.CreateSale;
using Ambev.Ominia.Application.Features.Sales.Commands.UpdateSale;
using Ambev.Ominia.Application.Features.Sales.Commands.CancelSale;
using Ambev.Ominia.Domain.Entities.Sales;
using Ambev.Ominia.Domain.Enums;
using Bogus;
using Shouldly;
using Xunit;

namespace Ambev.Ominia.Application.Tests.Features.Sales.Profiles;

/// <summary>
/// Tests for <see cref="SaleProfile"/>
/// </summary>
public class SaleProfileTests
{
    private readonly IMapper _mapper;
    private readonly Faker _faker;

    public SaleProfileTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<SalesProfile>());
        _mapper = configuration.CreateMapper();
        _faker = new Faker();
    }

    [Fact]
    public void Map_SaleItemToSaleItemDto_ShouldMapCorrectly()
    {
        // Arrange
        var saleItem = new SaleItem(
            saleId: 1,
            productId: 2,
            quantity: 5,
            unitPrice: 10.50m,
            discount: 2.00m);

        // Act
        var result = _mapper.Map<SaleItemDto>(saleItem);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(saleItem.Id);
        result.Quantity.ShouldBe(saleItem.Quantity);
        result.UnitPrice.ShouldBe(saleItem.UnitPrice);
        result.Total.ShouldBe(saleItem.Total);
    }

    [Fact]
    public void Map_SaleToSaleDto_ShouldMapCorrectly()
    {
        // Arrange
        var sale = new Sale(
            saleNumber: "SALE-001",
            date: DateTime.Now,
            customer: "Customer Test",
            branch: "Branch Test",
            items:
                [
                    new SaleItem(1, 1, 2, 15.00m, 0m),
                    new SaleItem(1, 2, 3, 20.00m, 5.00m)
                ]);

        // Act
        var result = _mapper.Map<SaleDto>(sale);

        // Assert
        result.ShouldNotBeNull();
        result.SaleNumber.ShouldBe(sale.SaleNumber);
        result.Date.ShouldBe(sale.Date);
        result.CustomerName.ShouldBe(sale.Customer);
        result.BranchName.ShouldBe(sale.Branch);
        result.Items.ShouldNotBeNull();
        result.Items.Count.ShouldBe(sale.Items.Count);
    }

    [Fact]
    public void Map_SaleToSaleSummaryDto_ShouldMapCorrectly()
    {
        // Arrange
        var sale = new Sale(
            saleNumber: "SALE-002",
            date: DateTime.Now,
            customer: "Customer Summary",
            branch: "Branch Summary",
            items: [new SaleItem(1, 1, 1, 25.00m, 0m)]);

        // Act
        var result = _mapper.Map<SaleSummaryDto>(sale);

        // Assert
        result.ShouldNotBeNull();
        result.SaleNumber.ShouldBe(sale.SaleNumber);
        result.Date.ShouldBe(sale.Date);
        result.CustomerName.ShouldBe(sale.Customer);
        result.BranchName.ShouldBe(sale.Branch);
    }

    [Fact]
    public void Map_SaleToCancelSaleResultDto_ShouldMapCorrectly()
    {
        // Arrange
        var sale = new Sale(
            saleNumber: "SALE-003",
            date: DateTime.Now,
            customer: "Customer Cancel",
            branch: "Branch Cancel",
            items: []);

        sale.Cancel();

        // Act
        var result = _mapper.Map<CancelSaleResultDto>(sale);

        // Assert
        result.ShouldNotBeNull();
        result.Status.ShouldBe(sale.Status.ToString());
    }

    [Fact]
    public void Map_CreateSaleCommandToSale_ShouldMapCorrectly()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            SaleNumber = "SALE-004",
            Date = DateTime.Now,
            Customer = "Command Customer",
            Branch = "Command Branch",
            Items =
                [
                new SaleItem(0, 1, 3, 12.50m, 1.00m),
                new SaleItem(0, 2, 2, 8.75m, 0m)
                ]
            };

        // Act
        var result = _mapper.Map<Sale>(command);

        // Assert
        result.ShouldNotBeNull();
        result.SaleNumber.ShouldBe(command.SaleNumber);
        result.Date.ShouldBe(command.Date);
        result.Customer.ShouldBe(command.Customer);
        result.Branch.ShouldBe(command.Branch);
        result.Items.ShouldNotBeNull();
        result.Items.Count.ShouldBe(command.Items.Count);

        var firstItem = result.Items.First();
        var firstCommandItem = command.Items.First();
        firstItem.ProductId.ShouldBe(firstCommandItem.ProductId);
        firstItem.Quantity.ShouldBe(firstCommandItem.Quantity);
        firstItem.UnitPrice.ShouldBe(firstCommandItem.UnitPrice);
        firstItem.Discount.ShouldBe(firstCommandItem.Discount);
    }

    [Fact]
    public void Map_UpdateSaleCommandToSale_ShouldMapCorrectly()
    {
        // Arrange
        var command = new UpdateSaleCommand
        {
            Id = 1,
            SaleNumber = "SALE-002",
            Date = DateTime.Now,
            Customer = "Updated Customer",
            Branch = "Updated Branch",
            Items = [new SaleItem(1, 1, 4, 18.00m, 2.50m)]
            };

        // Act
        var result = _mapper.Map<Sale>(command);

        // Assert
        result.ShouldNotBeNull();
        result.Customer.ShouldBe(command.Customer);
        result.Branch.ShouldBe(command.Branch);
        result.Items.ShouldNotBeNull();
        result.Items.Count.ShouldBe(command.Items.Count);

        var firstItem = result.Items.First();
        var firstCommandItem = command.Items.First();
        firstItem.ProductId.ShouldBe(firstCommandItem.ProductId);
        firstItem.Quantity.ShouldBe(firstCommandItem.Quantity);
        firstItem.UnitPrice.ShouldBe(firstCommandItem.UnitPrice);
        firstItem.Discount.ShouldBe(firstCommandItem.Discount);
    }

    [Fact]
    public void Map_SaleWithEmptyItems_ShouldMapCorrectly()
    {
        // Arrange
        var sale = new Sale(
            saleNumber: "SALE-005",
            date: DateTime.Now,
            customer: "Empty Items Customer",
            branch: "Empty Items Branch",
            items: []);

        // Act
        var saleDto = _mapper.Map<SaleDto>(sale);
        var saleSummaryDto = _mapper.Map<SaleSummaryDto>(sale);

        // Assert
        saleDto.ShouldNotBeNull();
        saleDto.Items.ShouldNotBeNull();
        saleDto.Items.Count.ShouldBe(0);
        saleSummaryDto.ShouldNotBeNull();
    }

    [Fact]
    public void Map_SaleItemWithZeroDiscount_ShouldMapCorrectly()
    {
        // Arrange
        var saleItem = new SaleItem(
            saleId: 1,
            productId: 3,
            quantity: 2,
            unitPrice: 15.00m,
            discount: 0m);

        // Act
        var result = _mapper.Map<SaleItemDto>(saleItem);

        // Assert
        result.ShouldNotBeNull();
        result.Total.ShouldBe(30.00m); // 15.00 * 2 - 0
    }

    [Fact]
    public void Map_SaleWithDifferentStatuses_ShouldMapCorrectly()
    {
        // Arrange
        var activeSale = new Sale("SALE-006", DateTime.Now, "Customer", "Branch", []);
        var cancelledSale = new Sale("SALE-007", DateTime.Now, "Customer", "Branch", []);
        cancelledSale.Cancel();

        // Act
        var activeResult = _mapper.Map<CancelSaleResultDto>(activeSale);
        var cancelledResult = _mapper.Map<CancelSaleResultDto>(cancelledSale);

        // Assert
        activeResult.Status.ShouldBe(SaleStatus.Active.ToString());
        cancelledResult.Status.ShouldBe(SaleStatus.Cancelled.ToString());
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        // Act & Assert
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<SalesProfile>());
        configuration.AssertConfigurationIsValid();
    }
}