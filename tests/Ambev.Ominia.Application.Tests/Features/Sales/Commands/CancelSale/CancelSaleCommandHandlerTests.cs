using Ambev.Ominia.Application.Features.Sales;
using Ambev.Ominia.Application.Features.Sales.Commands.CancelSale;
using Ambev.Ominia.Application.Tests.Features.Sales.Builders;
using Ambev.Ominia.Domain.Aggregates;
using Ambev.Ominia.Domain.Entities.Sales;
using Ambev.Ominia.Domain.Events.Sales;
using Ambev.Ominia.Domain.Handlers;
using Ambev.Ominia.Domain.Interfaces.Infrastructure;
using AutoMapper;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Ambev.Ominia.Application.Tests.Features.Sales.Commands.CancelSale;

public class CancelSaleCommandHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventSourcingHandler<SaleAggregate> _eventSourcingHandler;
    private readonly IEventPublisher _eventPublisher;
    private readonly CancelSaleCommandHandler _handler;

    public CancelSaleCommandHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _eventSourcingHandler = Substitute.For<IEventSourcingHandler<SaleAggregate>>();
        _eventPublisher = Substitute.For<IEventPublisher>();
        _handler = new CancelSaleCommandHandler(_saleRepository, _mapper, _eventSourcingHandler, _eventPublisher);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCancelSaleSuccessfully()
    {
        // Arrange
        var command = new CancelSaleCommandBuilder().WithId(1).Build();
        var existingSale = new SaleBuilder().Build();
        var expectedDto = new SaleDto { Id = 1, SaleNumber = existingSale.SaleNumber };

        // Simulate repository setting the ID
        typeof(Sale).GetProperty("Id")?.SetValue(existingSale, 1);

        var saleAggregate = SaleAggregate.Create(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            1,
            "SALE-001",
            DateTime.UtcNow,
            "Test Customer",
            "Test Branch",
            []);
        
        saleAggregate.Version = 1;
        
        _saleRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(existingSale);
        _saleRepository.UpdateAsync(existingSale, Arg.Any<CancellationToken>()).Returns(existingSale);
        _eventSourcingHandler.GetByIdAsync(Arg.Any<Guid>()).Returns(saleAggregate); // Return existing aggregate
        _mapper.Map<SaleDto>(existingSale).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(1);
        result.SaleNumber.ShouldBe(existingSale.SaleNumber);

        await _saleRepository.Received(1).GetByIdAsync(1, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(existingSale, Arg.Any<CancellationToken>());
        await _eventSourcingHandler.Received(1).GetByIdAsync(Arg.Any<Guid>());
        await _eventSourcingHandler.Received(1).SaveAsync(Arg.Any<SaleAggregate>());
        await _eventPublisher.Received(1).PublishAsync(Arg.Any<SaleCancelledEvent>());
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCallCancelOnSale()
    {
        // Arrange
        var command = new CancelSaleCommandBuilder().WithId(1).Build();
        var existingSale = Substitute.For<Sale>();
        var saleAggregate = SaleAggregate.Create(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            1,
            "SALE-001",
            DateTime.UtcNow,
            "Test Customer",
            "Test Branch",
            []);
        
        saleAggregate.Version = 1;
        
        var expectedDto = new SaleDto();

        _saleRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(existingSale);
        _saleRepository.UpdateAsync(existingSale, Arg.Any<CancellationToken>()).Returns(existingSale);
        _eventSourcingHandler.GetByIdAsync(Arg.Any<Guid>()).Returns(saleAggregate);
        _mapper.Map<SaleDto>(existingSale).Returns(expectedDto);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        existingSale.Received(1).Cancel();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCancelAggregateAndPublishEvent()
    {
        // Arrange
        var command = new CancelSaleCommandBuilder().WithId(1).Build();
        var existingSale = new SaleBuilder().Build();
        var expectedDto = new SaleDto();

        typeof(Sale).GetProperty("Id")?.SetValue(existingSale, 1);

        var saleAggregate = SaleAggregate.Create(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            1,
            "SALE-001",
            DateTime.UtcNow,
            "Test Customer",
            "Test Branch",
            []);
        
        saleAggregate.Version = 1;
        
        _saleRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(existingSale);
        _saleRepository.UpdateAsync(existingSale, Arg.Any<CancellationToken>()).Returns(existingSale);
        _eventSourcingHandler.GetByIdAsync(Arg.Any<Guid>()).Returns(saleAggregate); // Return existing aggregate
        _mapper.Map<SaleDto>(existingSale).Returns(expectedDto);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _eventSourcingHandler.Received(1).SaveAsync(Arg.Any<SaleAggregate>());
        await _eventPublisher.Received(1).PublishAsync(Arg.Any<SaleCancelledEvent>());
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldGenerateCorrectAggregateId()
    {
        // Arrange
        var saleId = 123;
        var command = new CancelSaleCommandBuilder().WithId(saleId).Build();
        var existingSale = new SaleBuilder().Build();
        var expectedDto = new SaleDto();
        var expectedGuid = Guid.Parse($"00000000-0000-0000-0000-{saleId:D12}");
        var saleAggregate = SaleAggregate.Create(
            expectedGuid,
            saleId,
            "SALE-123",
            DateTime.UtcNow,
            "Test Customer",
            "Test Branch",
            []);
        
        saleAggregate.Version = 1;

        typeof(Sale).GetProperty("Id")?.SetValue(existingSale, saleId);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(existingSale);
        _saleRepository.UpdateAsync(existingSale, Arg.Any<CancellationToken>()).Returns(existingSale);
        _eventSourcingHandler.GetByIdAsync(expectedGuid).Returns(saleAggregate);
        _mapper.Map<SaleDto>(existingSale).Returns(expectedDto);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _eventSourcingHandler.Received(1).GetByIdAsync(expectedGuid);
    }
}