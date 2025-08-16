using Ambev.Ominia.Application.Features.Products.Commands.CreateProduct;
using Ambev.Ominia.Application.Features.Products.Commands.UpdateProduct;
using Ambev.Ominia.Domain.Entities.Products;

namespace Ambev.Ominia.Application.Features.Products;

public class ProductsProfile : Profile
{
    public ProductsProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<Product, ProductSummaryDto>();

        CreateMap<CreateProductCommand, Product>();
        CreateMap<UpdateProductCommand, Product>();
    }
}