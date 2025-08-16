using Ambev.Ominia.Domain.Entities.Users;
using Ambev.Ominia.Domain.Entities.Products;
using Ambev.Ominia.Domain.Entities.Sales;
using Ambev.Ominia.Domain.Entities.Cart;
using Ambev.Ominia.Domain.Enums;
using Ambev.Ominia.Domain.ValueObjects;
using Ambev.Ominia.Crosscutting.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ambev.Ominia.Persistence;

public class DataSeeder(PostgreeContext context, IPasswordHasher passwordHasher, ILogger<DataSeeder> logger)
    {
    public async Task SeedAsync()
    {
        try
        {
            logger.LogInformation("Starting data seeding...");

            // Check if data already exists
            if (await context.Users.AnyAsync())
            {
                logger.LogInformation("Data already exists. Skipping seed.");
                return;
            }

            await SeedUsersAsync();
            await SeedProductsAsync();
            await context.SaveChangesAsync(); // Save first to get IDs
            
            await SeedCartsAsync();
            await SeedSalesAsync();

            await context.SaveChangesAsync();
            logger.LogInformation("Data seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during data seeding");
            throw;
        }
    }

    private async Task SeedUsersAsync()
    {
        var users = new List<User>
        {
            new User
            {
                Username = "admin",
                Email = "admin@ambev.com",
                Password = passwordHasher.HashPassword("Admin123!"),
                Phone = "+5511999999999",
                Status = UserStatus.Active,
                Role = UserRole.Admin
            },
            new User
            {
                Username = "manager",
                Email = "manager@ambev.com",
                Password = passwordHasher.HashPassword("Manager123!"),
                Phone = "+5511888888888",
                Status = UserStatus.Active,
                Role = UserRole.Manager
            },
            new User
            {
                Username = "customer",
                Email = "customer@ambev.com",
                Password = passwordHasher.HashPassword("Customer123!"),
                Phone = "+5511777777777",
                Status = UserStatus.Active,
                Role = UserRole.Customer
            }
        };

        await context.Users.AddRangeAsync(users);
        logger.LogInformation($"Seeded {users.Count} users");
    }

    private async Task SeedProductsAsync()
    {
        var products = new List<Product>
        {
            new Product
            {
                Title = "Brahma Duplo Malte",
                Description = "Cerveja Brahma Duplo Malte 350ml",
                Category = "Cerveja",
                Price = 4.20m,
                Rating = new Rating(4.5, 200),
                Image = "https://example.com/brahma-duplo-malte.jpg"
            },
            new Product
            {
                Title = "Skol Pilsen",
                Description = "Cerveja Skol Pilsen 350ml",
                Category = "Cerveja",
                Price = 3.50m,
                Rating = new Rating(4.2, 150),
                Image = "https://example.com/skol-pilsen.jpg"
            },
            new Product
            {
                Title = "Antarctica Original",
                Description = "Cerveja Antarctica Original 350ml",
                Category = "Cerveja",
                Price = 3.80m,
                Rating = new Rating(4.3, 180),
                Image = "https://example.com/antarctica-original.jpg"
            },
            new Product
            {
                Title = "Stella Artois",
                Description = "Cerveja Stella Artois 330ml",
                Category = "Cerveja Premium",
                Price = 6.50m,
                Rating = new Rating(4.7, 120),
                Image = "https://example.com/stella-artois.jpg"
            },
            new Product
            {
                Title = "Corona Extra",
                Description = "Cerveja Corona Extra 355ml",
                Category = "Cerveja Importada",
                Price = 8.90m,
                Rating = new Rating(4.6, 90),
                Image = "https://example.com/corona-extra.jpg"
            },
            new Product
            {
                Title = "Heineken Green Power",
                Description = "Cerveja Heineken com energia verde 330ml",
                Category = "Cerveja Premium",
                Price = 7.20m,
                Rating = new Rating(4.4, 85),
                Image = "https://example.com/heineken-green.jpg"
            },
            new Product
            {
                Title = "Budweiser King Size",
                Description = "Cerveja Budweiser tamanho rei 473ml",
                Category = "Cerveja Importada",
                Price = 9.50m,
                Rating = new Rating(4.3, 110),
                Image = "https://example.com/budweiser-king.jpg"
            },
            new Product
            {
                Title = "Guaraná Antarctica Power",
                Description = "Refrigerante Guaraná Antarctica energético 350ml",
                Category = "Refrigerante",
                Price = 3.20m,
                Rating = new Rating(4.6, 250),
                Image = "https://example.com/guarana-power.jpg"
            },
            new Product
            {
                Title = "Coca-Cola Zero Sugar",
                Description = "Refrigerante Coca-Cola sem açúcar 350ml",
                Category = "Refrigerante",
                Price = 3.80m,
                Rating = new Rating(4.2, 300),
                Image = "https://example.com/coca-zero.jpg"
            },
            new Product
            {
                Title = "Pepsi Twist Limão",
                Description = "Refrigerante Pepsi sabor limão 350ml",
                Category = "Refrigerante",
                Price = 3.50m,
                Rating = new Rating(4.1, 180),
                Image = "https://example.com/pepsi-twist.jpg"
            },
            new Product
            {
                Title = "Água Crystal Mountain",
                Description = "Água mineral Crystal da montanha 500ml",
                Category = "Água",
                Price = 2.50m,
                Rating = new Rating(4.8, 400),
                Image = "https://example.com/crystal-mountain.jpg"
            },
            new Product
            {
                Title = "Suco Fresh Orange",
                Description = "Suco natural de laranja fresco 300ml",
                Category = "Suco Natural",
                Price = 5.20m,
                Rating = new Rating(4.7, 95),
                Image = "https://example.com/fresh-orange.jpg"
            },
            new Product
            {
                Title = "Energético Red Bull Wings",
                Description = "Bebida energética Red Bull com asas 250ml",
                Category = "Energético",
                Price = 8.50m,
                Rating = new Rating(4.5, 150),
                Image = "https://example.com/redbull-wings.jpg"
            },
            new Product
            {
                Title = "Whisky Black Label Premium",
                Description = "Whisky escocês Black Label premium 750ml",
                Category = "Destilado",
                Price = 89.90m,
                Rating = new Rating(4.9, 45),
                Image = "https://example.com/black-label.jpg"
            },
            new Product
            {
                Title = "Vodka Absolut Crystal",
                Description = "Vodka Absolut cristalina premium 750ml",
                Category = "Destilado",
                Price = 75.50m,
                Rating = new Rating(4.6, 60),
                Image = "https://example.com/absolut-crystal.jpg"
            },
            new Product
            {
                Title = "Vinho Tinto Reserva Special",
                Description = "Vinho tinto reserva especial 750ml",
                Category = "Vinho",
                Price = 45.80m,
                Rating = new Rating(4.4, 75),
                Image = "https://example.com/vinho-reserva.jpg"
            },
            new Product
            {
                Title = "Champagne Celebration Gold",
                Description = "Champagne para celebração dourada 750ml",
                Category = "Espumante",
                Price = 120.00m,
                Rating = new Rating(4.8, 30),
                Image = "https://example.com/champagne-gold.jpg"
            },
            new Product
            {
                Title = "Caipirinha Mix Ready",
                Description = "Mix pronto para caipirinha tradicional 1L",
                Category = "Coquetel",
                Price = 15.90m,
                Rating = new Rating(4.3, 120),
                Image = "https://example.com/caipirinha-mix.jpg"
            },
            new Product
            {
                Title = "Cerveja Artesanal Hoppy Dreams",
                Description = "Cerveja artesanal dos sonhos lupulados 500ml",
                Category = "Cerveja Artesanal",
                Price = 12.50m,
                Rating = new Rating(4.7, 85),
                Image = "https://example.com/hoppy-dreams.jpg"
            },
            new Product
            {
                Title = "Kombucha Tropical Paradise",
                Description = "Kombucha sabor paraíso tropical 350ml",
                Category = "Bebida Funcional",
                Price = 8.90m,
                Rating = new Rating(4.5, 65),
                Image = "https://example.com/kombucha-tropical.jpg"
            }
        };

        await context.Products.AddRangeAsync(products);
        logger.LogInformation($"Seeded {products.Count} products");
    }

    private async Task SeedCartsAsync()
    {
        var users = await context.Users.ToListAsync();
        var products = await context.Products.ToListAsync();

        if (!users.Any() || !products.Any())
        {
            logger.LogWarning("No users or products found for creating carts");
            return;
        }

        var random = new Random();
        var carts = new List<Cart>();

        // Create 10 carts with different scenarios
        for (int i = 0; i < 10; i++)
        {
            var user = users[i % users.Count];
            var cartProducts = products.OrderBy(x => random.Next()).Take(random.Next(2, 4)).ToList();
            
            var cart = new Cart
            {
                UserId = user.Id,
                Date = DateTime.UtcNow.AddDays(-random.Next(0, 7)),
                CartItems = []
                };

            foreach (var product in cartProducts)
            {
                var cartItem = new CartItem
                {
                    ProductId = product.Id,
                    Quantity = random.Next(1, 4)
                };
                cart.CartItems.Add(cartItem);
            }

            carts.Add(cart);
        }

        await context.Carts.AddRangeAsync(carts);
        logger.LogInformation($"Seeded {carts.Count} carts with {carts.Sum(c => c.CartItems.Count)} items");
    }

    private async Task SeedSalesAsync()
    {
        // Get users and products for creating sales
        var users = await context.Users.ToListAsync();
        var products = await context.Products.ToListAsync();

        if (!users.Any() || !products.Any())
        {
            logger.LogWarning("No users or products found for creating sales");
            return;
        }

        var random = new Random();
        var sales = new List<Sale>();

        // Create 30 sales with different scenarios
        for (int i = 0; i < 30; i++)
        {
            var user = users[i % users.Count];
            var saleProducts = products.OrderBy(x => random.Next()).Take(random.Next(2, 4)).ToList();
            
            var saleItems = saleProducts.Select(p => {
                var saleItem = new SaleItem(
                    0, // SaleId - Will be set by EF Core
                    p.Id, // ProductId
                    random.Next(1, 4), // Quantity
                    p.Price, // UnitPrice
                    random.Next(0, 15) / 100m // Discount - 0% to 15%
                );
                
                // 20% chance of individual item being cancelled
                if (random.Next(0, 5) == 0)
                {
                    saleItem.Cancel();
                }
                
                return saleItem;
            }).ToList();

            var sale = new Sale(
                $"SALE-{DateTime.UtcNow:yyyyMMdd}-{i + 1:D3}",
                DateTime.UtcNow.AddDays(-random.Next(0, 15)),
                user.Username,
                $"Filial {(i % 3 == 0 ? "Centro" : i % 3 == 1 ? "Norte" : "Sul")}",
                saleItems
            );

            // 30% chance of entire sale being cancelled
            if (random.Next(0, 10) < 3)
            {
                sale.Cancel();
            }

            sales.Add(sale);
        }

        await context.Sales.AddRangeAsync(sales);
        logger.LogInformation($"Seeded {sales.Count} sales with {sales.Sum(s => s.Items.Count)} items");
        
        var cancelledSales = sales.Count(s => !s.IsActive());
        var cancelledItems = sales.SelectMany(s => s.Items).Count(i => !i.IsActive());
        logger.LogInformation($"Cancelled sales: {cancelledSales}, Cancelled items: {cancelledItems}");
    }
}