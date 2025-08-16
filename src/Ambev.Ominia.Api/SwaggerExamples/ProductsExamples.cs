using Swashbuckle.AspNetCore.Filters;
using Ambev.Ominia.Application.Features.Products.Commands.CreateProduct;
using Ambev.Ominia.Application.Features.Products.Commands.UpdateProduct;

namespace Ambev.Ominia.Api.SwaggerExamples;

public class CreateProductCommandExample : IExamplesProvider<CreateProductCommand>
{
    public CreateProductCommand GetExamples()
    {
        return new CreateProductCommand
        {
            Title = "Premium Craft Beer",
            Price = 15.99m,
            Description = "A high-quality craft beer with exceptional taste and aroma",
            Image = "https://example.com/images/premium-beer.jpg",
            Category = "Beverages"
        };
    }
}

public class UpdateProductCommandExample : IExamplesProvider<UpdateProductCommand>
{
    public UpdateProductCommand GetExamples()
    {
        return new UpdateProductCommand
        {
            Id = 6,
            Title = "Updated Premium Craft Beer",
            Price = 27.99m,
            Description = "An updated high-quality craft beer with enhanced flavor profile",
            Image = "https://example.com/images/updated-premium-beer.jpg",
            Category = "Beverages"
        };
    }
}