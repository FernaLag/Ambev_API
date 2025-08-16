using Ambev.Ominia.Domain.Entities.Cart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.Ominia.Persistence.Configurations;

/// <summary>
/// Configuration class for the CartItem entity.
/// Defines the table structure and constraints for Entity Framework.
/// </summary>
public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    /// <summary>
    /// Configures the CartItem entity mapping.
    /// </summary>
    /// <param name="builder">EntityTypeBuilder for CartItem.</param>
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems");

        builder.HasKey(cp => cp.Id);

        builder.Property(cp => cp.Quantity)
            .IsRequired()
            .HasDefaultValue(1);

        builder.HasOne(cp => cp.Cart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(cp => cp.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cp => cp.Product)
            .WithMany()
            .HasForeignKey(cp => cp.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}