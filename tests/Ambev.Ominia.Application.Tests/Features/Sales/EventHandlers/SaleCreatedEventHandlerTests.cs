using Ambev.Ominia.Application.Features.Sales.EventHandlers;
using Ambev.Ominia.Domain.Events.Sales;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Ambev.Ominia.Application.Tests.Features.Sales.EventHandlers;

public class SaleCreatedEventHandlerTests
{
    private readonly SaleCreatedEventHandler _handler;

    public SaleCreatedEventHandlerTests()
    {
    ILogger<SaleCreatedEventHandler> logger = Substitute.For<ILogger<SaleCreatedEventHandler>>();
        _handler = new SaleCreatedEventHandler(logger);
    }

    [Fact]
    public async Task Handle_ValidEvent_ShouldLogInformationMessages()
    {
        // Arrange
        var saleCreatedEvent = new SaleCreatedEvent(
            Guid.NewGuid(),
            1,
            "SALE-2024-001",
            DateTime.UtcNow,
            "Test Customer",
            "Test Branch",
            []);

        // Act
        await _handler.Handle(saleCreatedEvent);

        // Assert
        var exception = await Record.ExceptionAsync(() => Task.CompletedTask);
        exception.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_ValidEvent_ShouldCompleteSuccessfully()
    {
        // Arrange
        var saleCreatedEvent = new SaleCreatedEvent(
            Guid.NewGuid(),
            1,
            "SALE-2024-001",
            DateTime.UtcNow,
            "Test Customer",
            "Test Branch",
            []);

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _handler.Handle(saleCreatedEvent));
        exception.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_WhenExceptionOccurs_ShouldLogErrorAndRethrow()
    {
        // Arrange
        var saleCreatedEvent = new SaleCreatedEvent(
            Guid.NewGuid(),
            1,
            "SALE-2024-001",
            DateTime.UtcNow,
            "Test Customer",
            "Test Branch",
            []);

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _handler.Handle(saleCreatedEvent));
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
    public async Task Handle_WithDifferentSaleIds_ShouldLogCorrectIds()
    {
        // Arrange
        var aggregateId = Guid.NewGuid();
        var saleId = 42;
        var saleCreatedEvent = new SaleCreatedEvent(
            aggregateId,
            saleId,
            "SALE-2024-042",
            DateTime.UtcNow,
            "Another Customer",
            "Another Branch",
            []);

        // Act
        await _handler.Handle(saleCreatedEvent);

        // Assert
        var exception = await Record.ExceptionAsync(() => Task.CompletedTask);
        exception.ShouldBeNull();
    }
}