using Ambev.Ominia.Application.Features.Carts.Commands.CreateCart;
using Ambev.Ominia.Domain.Entities.Cart;

namespace Ambev.Ominia.Application.Features.Carts;

public class CartsProfile : Profile
{
    public CartsProfile()
    {
        CreateMap<CartItem, CartItemDto>()
            .ForMember(d => d.ProductTitle, opt => opt.MapFrom(s => s.Product != null ? s.Product.Title : string.Empty))
            .ForMember(d => d.ProductPrice, opt => opt.MapFrom(s => s.Product != null ? s.Product.Price : 0))
            .ForMember(d => d.ProductImage, opt => opt.MapFrom(s => s.Product != null ? s.Product.Image : string.Empty));

        CreateMap<Cart, CartDto>();
        CreateMap<Cart, CartSummaryDto>();

        // Command mappings
        CreateMap<CreateCartCommand, Cart>()
            .ForMember(d => d.CartItems, opt => opt.MapFrom(s => s.Products));
        CreateMap<CreateCartItemCommand, CartItem>()
            .ForMember(d => d.Cart, opt => opt.Ignore())
            .ForMember(d => d.Product, opt => opt.Ignore());
    }
}