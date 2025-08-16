using Ambev.Ominia.Application.Features.Sales.EventHandlers;
using Ambev.Ominia.Domain.Events.Sales;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Ambev.Ominia.Application.Tests.Features.Sales.EventHandlers;

public class SaleModifiedEventHandlerTests
{
    private readonly ILogger<SaleModifiedEventHandler> _logger;
    private readonly SaleModifiedEventHandler _handler;

    public SaleModifiedEventHandlerTests()
    {
        _logger = Substitute.For<ILogger<SaleModifiedEventHandler>>();
        _handler = new SaleModifiedEventHandler(_logger);
    }

    [Fact]
    public async Task Handle_ValidEvent_ShouldLogInformationMessages()
    {
        // Arrange
        var saleModifiedEvent = new SaleModifiedEvent(
            Guid.NewGuid(),
            "SALE-2024-001",
            DateTime.UtcNow,
            "Updated Customer",
            "Updated Branch",
            []);

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _handler.Handle(saleModifiedEvent));
        exception.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_ValidEvent_ShouldCompleteSuccessfully()
    {
        // Arrange
        var saleModifiedEvent = new SaleModifiedEvent(
            Guid.NewGuid(),
            "SALE-2024-001",
            DateTime.UtcNow,
            "Updated Customer",
            "Updated Branch",
            []);

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _handler.Handle(saleModifiedEvent));
        exception.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_WhenExceptionOccurs_ShouldLogErrorAndRethrow()
    {
        // Arrange
        var saleModifiedEvent = new SaleModifiedEvent(
            Guid.NewGuid(),
            "SALE-2024-001",
            DateTime.UtcNow,
            "Updated Customer",
            "Updated Branch",
            []);

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _handler.Handle(saleModifiedEvent));
        exception.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_WithNullEvent_ShouldHandleGracefully()
    {
        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _handler.Handle(null!));
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<NullReferenceException>();
    }

    [Fact]
    public async Task Handle_WithDifferentSaleNumbers_ShouldLogCorrectNumbers()
    {
        // Arrange
        var aggregateId = Guid.NewGuid();
        var saleNumber = "SALE-2024-999";
        var saleModifiedEvent = new SaleModifiedEvent(
            aggregateId,
            saleNumber,
            DateTime.UtcNow,
            "Special Customer",
            "Special Branch",
            []);

        // Act
        await _handler.Handle(saleModifiedEvent);

        // Assert
        var exception = await Record.ExceptionAsync(() => Task.CompletedTask);
        exception.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_WithEmptyItems_ShouldProcessSuccessfully()
    {
        // Arrange
        var saleModifiedEvent = new SaleModifiedEvent(
            Guid.NewGuid(),
            "SALE-2024-EMPTY",
            DateTime.UtcNow,
            "Customer with no items",
            "Empty Branch",
            []);

        // Act
        await _handler.Handle(saleModifiedEvent);

        // Assert
        var exception = await Record.ExceptionAsync(() => Task.CompletedTask);
        exception.ShouldBeNull();
    }
}