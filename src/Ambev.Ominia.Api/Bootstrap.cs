using Ambev.Ominia.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ambev.Ominia.Application;
using Ambev.Ominia.Crosscutting.Behaviors;
using Ambev.Ominia.Crosscutting.HealthChecks;
using Ambev.Ominia.Crosscutting.Logging;
using Ambev.Ominia.Crosscutting.Middleware;
using Ambev.Ominia.Crosscutting.Security;
using Ambev.Ominia.Domain.Interfaces.Infrastructure;
using Ambev.Ominia.Persistence.Repositories;
using Ambev.Ominia.EventStore.Repositories;
using Ambev.Ominia.Domain.Interfaces;
using Ambev.Ominia.Domain.Specifications;
using Ambev.Ominia.Domain.Entities.Users;
using Ambev.Ominia.Messaging.Configurations;
using Ambev.Ominia.Domain.Events;
using Ambev.Ominia.Domain.Handlers;
using Ambev.Ominia.EventStore;
using Ambev.Ominia.EventStore.Services;
using Swashbuckle.AspNetCore.Filters;
using Ambev.Ominia.Api.SwaggerExamples;
using Ambev.Ominia.Domain.Events.Sales;

namespace Ambev.Ominia.Api;

public static class Bootstrap
{
    private const string CorsPolicyName = "_myAllowSpecificOrigins";

    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.AddDefaultLogging();

        var services = builder.Services;
        var configuration = builder.Configuration;

        AddApi(services);
        AddCors(services);
        AddPersistence(services, configuration);
        AddSecurity(services, configuration);
        AddMessaging(services, configuration);
        AddEventStore(services, configuration);
        AddApplicationLayer(services);
    }

    public static void ConfigureMiddleware(WebApplication app)
    {
        RunEfMigrations(app);

        // Configure Rebus event subscriptions
        var bus = app.Services.GetRequiredService<Rebus.Bus.IBus>();
        bus.Subscribe<SaleCreatedEvent>();
        bus.Subscribe<SaleModifiedEvent>();

        app.UseMiddleware<ValidationExceptionMiddleware>();

        if (app.Environment.IsDevelopment())
            UseDeveloperTools(app);

        app.UseHttpsRedirection();

        app.UseCors(CorsPolicyName);

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseBasicHealthChecks();
        app.MapControllers();
    }

    private static void AddApi(IServiceCollection services)
    {
        services.AddControllers(options =>
            {
                options.Filters.Add(new Microsoft.AspNetCore.Mvc.ProducesAttribute("application/json"));
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            });
        services.AddHealthChecks();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(t => t.FullName);

            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                        []
                }
            });

            c.ExampleFilters();
        });

        services.AddSwaggerExamplesFromAssemblyOf<AuthenticateCommandExample>();
    }

    private static void AddCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, policy => policy
                .WithOrigins("http://localhost:8080", "https://localhost:8080")
                .AllowAnyMethod()
                .AllowCredentials()
                .AllowAnyHeader()
                .WithHeaders("Authorization", "Content-Type", "Accept"));
        });
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PostgreeContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("PostgreeConnection");
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorCodesToAdd: null);
            });
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<PostgreeContext>());

        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void AddSecurity(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        services.AddJwtAuthentication(configuration);
        services.AddScoped<ISpecification<User>, ActiveUserSpecification>();
    }

    private static void AddEventStore(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbConfig>(configuration.GetSection("MongoDbConfig"));
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped(typeof(IEventService<>), typeof(EventService<>));
        services.AddScoped(typeof(IEventSourcingHandler<>), typeof(EventSourcingHandler<>));
    }

    private static void AddMessaging(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMessagingServices(configuration);
    }

    private static void AddApplicationLayer(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ApplicationLayer).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
        services.AddAutoMapper(typeof(ApplicationLayer).Assembly);
    }

    private static void RunEfMigrations(WebApplication app)
    {
        var runMigrations = app.Configuration.GetValue<bool>("RunMigrations");
        Console.WriteLine($"RunMigrations configuration value: {runMigrations}");
        
        if (!runMigrations) 
        {
            Console.WriteLine("Skipping migrations - RunMigrations is false");
            return;
        }

        Console.WriteLine("Starting database migrations...");
        
        // Retry logic for database connection
        const int maxRetries = 10;
        const int delayMs = 5000;
        
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                Console.WriteLine($"Database connection attempt {attempt}/{maxRetries}...");
                
                using var scope = app.Services.CreateScope();
                var serviceProvider = scope.ServiceProvider;

                // Get the DbContext options first
                var options = serviceProvider.GetRequiredService<DbContextOptions<PostgreeContext>>();

                // Create a new instance of the context with the options
                using var db = new PostgreeContext(options);

                // Test connection first
                db.Database.CanConnect();
                Console.WriteLine("Database connection successful!");

                // Run migrations
                Console.WriteLine("Running database migrations...");
                db.Database.Migrate();
                Console.WriteLine("Database migrations completed successfully.");

                // Run data seeding if enabled
                var runDataSeed = app.Configuration.GetValue<bool>("RunDataSeed");
                Console.WriteLine($"RunDataSeed configuration value: {runDataSeed}");
                
                if (runDataSeed)
                {
                    Console.WriteLine("Starting data seeding...");
                    var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();
                    var logger = serviceProvider.GetRequiredService<ILogger<DataSeeder>>();
                    var seeder = new DataSeeder(db, passwordHasher, logger);
                    seeder.SeedAsync().Wait();
                    Console.WriteLine("Data seeding completed successfully.");
                }
                else
                {
                    Console.WriteLine("Skipping data seeding - RunDataSeed is false");
                }
                
                // If we reach here, everything was successful
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Migration attempt {attempt} failed: {ex.Message}");
                
                if (attempt == maxRetries)
                {
                    Console.WriteLine($"All {maxRetries} migration attempts failed. Last error: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    return;
                }
                
                Console.WriteLine($"Waiting {delayMs}ms before retry...");
                Thread.Sleep(delayMs);
            }
        }
    }

    private static void UseDeveloperTools(WebApplication app)
    {
        // Swagger only in Development
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}