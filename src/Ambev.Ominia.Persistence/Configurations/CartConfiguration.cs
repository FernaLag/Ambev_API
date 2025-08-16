using Ambev.Ominia.Domain.Entities.Cart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.Ominia.Persistence.Configurations;

/// <summary>
/// Configuration class for the Cart entity.
/// Defines the table structure and constraints for Entity Framework.
/// </summary>
public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    /// <summary>
    /// Configures the Cart entity mapping.
    /// </summary>
    /// <param name="builder">EntityTypeBuilder for Cart.</param>
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.UserId)
            .IsRequired();

        builder.Property(c => c.Date)
            .IsRequired();

        builder.HasMany(c => c.CartItems)
            .WithOne(cp => cp.Cart)
            .HasForeignKey(cp => cp.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(c => c.CartItems).AutoInclude();
    }
}