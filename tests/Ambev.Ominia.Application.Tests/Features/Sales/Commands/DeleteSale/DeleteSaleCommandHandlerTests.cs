using Ambev.Ominia.Application.Features.Sales.Commands.DeleteSale;
using Ambev.Ominia.Application.Tests.Features.Sales.Builders;
using Ambev.Ominia.Domain.Interfaces.Infrastructure;
using NSubstitute;
using Xunit;

namespace Ambev.Ominia.Application.Tests.Features.Sales.Commands.DeleteSale;

public class DeleteSaleCommandHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly DeleteSaleCommandHandler _handler;

    public DeleteSaleCommandHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new DeleteSaleCommandHandler(_saleRepository);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldDeleteSaleSuccessfully()
    {
        // Arrange
        var command = new DeleteSaleCommandBuilder().WithId(1).Build();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleRepository.Received(1).DeleteAsync(1, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCallRepositoryWithCorrectId()
    {
        // Arrange
        var saleId = 123;
        var command = new DeleteSaleCommandBuilder().WithId(saleId).Build();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleRepository.Received(1).DeleteAsync(saleId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldPassCancellationToken()
    {
        // Arrange
        var command = new DeleteSaleCommandBuilder().Build();
        var cancellationToken = new CancellationToken();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        await _saleRepository.Received(1).DeleteAsync(command.Id, cancellationToken);
    }
}