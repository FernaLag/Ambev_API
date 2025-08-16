using Ambev.Ominia.Application.Features.Sales.Commands.CreateSale;
using Ambev.Ominia.Application.Tests.Features.Sales.Builders;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.Ominia.Application.Tests.Features.Sales.Commands.CreateSale;

public class CreateSaleCommandValidatorTests
{
    private readonly CreateSaleCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateSaleCommandBuilder()
            .WithSaleNumber("SALE-001")
            .WithCustomer("Customer Name")
            .WithBranch("Branch Name")
            .WithItems([
                new SaleItemBuilder()
                    .WithQuantity(5)
                    .WithUnitPrice(10.00m)
                    .Build()
                ])
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_InvalidSaleNumber_ShouldHaveValidationError(string saleNumber)
    {
        // Arrange
        var command = new CreateSaleCommandBuilder()
            .WithSaleNumber(saleNumber)
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SaleNumber);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_InvalidCustomer_ShouldHaveValidationError(string customer)
    {
        // Arrange
        var command = new CreateSaleCommandBuilder()
            .WithCustomer(customer)
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Customer);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_InvalidBranch_ShouldHaveValidationError(string branch)
    {
        // Arrange
        var command = new CreateSaleCommandBuilder()
            .WithBranch(branch)
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Branch);
    }

    [Fact]
    public void Validate_EmptyItems_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateSaleCommandBuilder()
            .WithItems([])
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Items);
    }

    [Fact]
    public void Validate_NullItems_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateSaleCommandBuilder()
            .WithItems(null!)
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Items);
    }

    [Fact]
    public void Validate_ItemsWithInvalidQuantity_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateSaleCommandBuilder()
            .WithItems([
                new SaleItemBuilder()
                    .WithQuantity(0)
                    .Build()
                ])
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].Quantity");
    }

    [Fact]
    public void Validate_ItemsWithInvalidUnitPrice_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateSaleCommandBuilder()
            .WithItems([
                new SaleItemBuilder()
                    .WithUnitPrice(0)
                    .Build()
                ])
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].UnitPrice");
    }

    [Fact]
    public void Validate_ItemsWithInvalidProduct_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateSaleCommandBuilder()
            .WithItems([
                new SaleItemBuilder()
                    .WithProductId(0) // Invalid ProductId
                    .Build()
                ])
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].ProductId");
    }
}