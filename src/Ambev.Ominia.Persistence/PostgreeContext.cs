using Ambev.Ominia.Domain.Entities.Cart;
using Ambev.Ominia.Domain.Entities.Products;
using Ambev.Ominia.Domain.Entities.Sales;
using Ambev.Ominia.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ambev.Ominia.Persistence;

public class PostgreeContext(DbContextOptions<PostgreeContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}