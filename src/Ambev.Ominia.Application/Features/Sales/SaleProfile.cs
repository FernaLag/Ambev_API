using Ambev.Ominia.Application.Features.Sales.Commands.CancelSale;
using Ambev.Ominia.Application.Features.Sales.Commands.CreateSale;
using Ambev.Ominia.Application.Features.Sales.Commands.UpdateSale;
using Ambev.Ominia.Domain.Entities.Sales;

namespace Ambev.Ominia.Application.Features.Sales;

public class SalesProfile : Profile
{
    public SalesProfile()
    {
        CreateMap<SaleItem, SaleItemDto>()
            .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product != null ? s.Product.Title : string.Empty))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

        CreateMap<Sale, SaleDto>()
            .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Customer))
            .ForMember(d => d.BranchName, opt => opt.MapFrom(s => s.Branch))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));
            
        CreateMap<Sale, SaleSummaryDto>()
            .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Customer))
            .ForMember(d => d.BranchName, opt => opt.MapFrom(s => s.Branch));

        CreateMap<Sale, CancelSaleResultDto>()
            .ConstructUsing(s => new CancelSaleResultDto(
                Guid.Parse($"00000000-0000-0000-0000-{s.Id:D12}"),
                s.Status.ToString()))
            .ForAllMembers(opt => opt.Ignore());
        
        // Command mappings
        CreateMap<CreateSaleCommand, Sale>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.SaleNumber, opt => opt.MapFrom(s => s.SaleNumber))
            .ForMember(d => d.Date, opt => opt.MapFrom(s => s.Date))
            .ForMember(d => d.Customer, opt => opt.MapFrom(s => s.Customer))
            .ForMember(d => d.Branch, opt => opt.MapFrom(s => s.Branch))
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items))
            .ForMember(d => d.Status, opt => opt.Ignore());
            
        CreateMap<UpdateSaleCommand, Sale>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.SaleNumber, opt => opt.MapFrom(s => s.SaleNumber))
            .ForMember(d => d.Date, opt => opt.MapFrom(s => s.Date))
            .ForMember(d => d.Customer, opt => opt.MapFrom(s => s.Customer))
            .ForMember(d => d.Branch, opt => opt.MapFrom(s => s.Branch))
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items))
            .ForMember(d => d.Status, opt => opt.Ignore());
    }
    
    /// <summary>
    /// Generates a deterministic GUID based on the sale ID.
    /// </summary>
    private static Guid GetDeterministicGuid(int saleId)
    {
        return Guid.Parse($"00000000-0000-0000-0000-{saleId:D12}");
    }
}