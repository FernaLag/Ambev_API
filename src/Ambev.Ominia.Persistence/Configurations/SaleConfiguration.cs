using Ambev.Ominia.Domain.Entities.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.Ominia.Persistence.Configurations;

/// <summary>
/// Configuration class for the Sale entity.
/// Defines the table structure and constraints for Entity Framework.
/// </summary>
public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.Property(s => s.SaleNumber).IsRequired();
        builder.Property(s => s.Date).IsRequired();
        builder.Property(s => s.Customer).IsRequired();
        builder.Property(s => s.Branch).IsRequired();

        builder.HasMany(s => s.Items).WithOne().OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(s => s.Items).AutoInclude();
        builder.Navigation(s => s.Items).EnableLazyLoading(false);
    }
}