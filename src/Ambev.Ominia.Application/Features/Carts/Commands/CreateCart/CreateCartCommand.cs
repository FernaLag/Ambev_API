namespace Ambev.Ominia.Application.Features.Carts.Commands.CreateCart;

public class CreateCartCommand : IRequest<CartDto>
{
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CreateCartItemCommand> Products { get; set; } = [];
}

public class CreateCartItemCommand
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}