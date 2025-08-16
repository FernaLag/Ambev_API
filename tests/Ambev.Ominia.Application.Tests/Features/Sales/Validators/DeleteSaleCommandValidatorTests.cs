using Ambev.Ominia.Application.Features.Sales.Commands.DeleteSale;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.Ominia.Application.Tests.Features.Sales.Validators;

/// <summary>
/// Tests for <see cref="DeleteSaleValidator"/>
/// </summary>
public class DeleteSaleCommandValidatorTests
{
    private readonly DeleteSaleValidator _validator = new();

    [Fact]
    public void Validate_ValidId_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new DeleteSaleCommand(1);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_InvalidId_ShouldHaveValidationError(int id)
    {
        // Arrange
        var command = new DeleteSaleCommand(id);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}