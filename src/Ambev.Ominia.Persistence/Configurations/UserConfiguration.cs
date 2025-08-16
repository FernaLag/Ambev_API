using Ambev.Ominia.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.Ominia.Persistence.Configurations;

/// <summary>
/// Configuration class for the User entity.
/// Defines the database table structure and constraints using Entity Framework.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configures the User entity mapping.
    /// </summary>
    /// <param name="builder">EntityTypeBuilder for User.</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Phone)
            .HasMaxLength(20);

        builder.Property(u => u.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.OwnsOne(u => u.Address, address =>
        {
            address.Property(a => a.City).HasMaxLength(100);
            address.Property(a => a.Street).HasMaxLength(100);
            address.Property(a => a.Number);
            address.Property(a => a.ZipCode).HasMaxLength(20);

            address.OwnsOne(a => a.Geolocation, geo =>
            {
                geo.Property(g => g.Latitude).HasMaxLength(50);
                geo.Property(g => g.Longitude).HasMaxLength(50);
            });
        });
    }
}