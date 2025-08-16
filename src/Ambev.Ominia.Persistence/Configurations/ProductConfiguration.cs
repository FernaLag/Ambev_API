using Ambev.Ominia.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.Ominia.Persistence.Configurations;

/// <summary>
/// Configuration class for the Product entity.
/// Defines the table structure and constraints for Entity Framework.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    /// <summary>
    /// Configures the Product entity mapping.
    /// </summary>
    /// <param name="builder">EntityTypeBuilder for Product.</param>
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Category)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Image)
            .HasMaxLength(255);

        builder.OwnsOne(p => p.Rating, rating =>
        {
            rating.Property(r => r.Rate)
                .HasColumnType("decimal(3,2)");

            rating.Property(r => r.Count)
                .HasColumnType("int");
        });

        builder.HasMany(p => p.CartItems)
            .WithOne(cp => cp.Product)
            .HasForeignKey(cp => cp.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}