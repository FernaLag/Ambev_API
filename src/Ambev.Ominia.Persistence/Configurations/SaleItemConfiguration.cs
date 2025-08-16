using Ambev.Ominia.Domain.Entities.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.Ominia.Persistence.Configurations;

/// <summary>
/// Configuration class for the SaleItem entity.
/// Defines the table structure and constraints for Entity Framework.
/// </summary>
public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    /// <summary>
    /// Configures the SaleItem entity mapping.
    /// </summary>
    /// <param name="builder">EntityTypeBuilder for SaleItem.</param>
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(si => si.Id);
        builder.Property(si => si.Id).ValueGeneratedOnAdd();

        builder.Property(si => si.Quantity)
            .IsRequired();

        builder.Property(si => si.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(si => si.Discount)
            .HasColumnType("decimal(18,2)");

        builder.Property(si => si.Status)
            .IsRequired();

        builder.HasOne(si => si.Product)
            .WithMany()
            .HasForeignKey(si => si.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(si => si.Product).AutoInclude();
    }
}