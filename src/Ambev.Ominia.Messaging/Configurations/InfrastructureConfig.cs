using Ambev.Ominia.Domain.Interfaces.Infrastructure;
using Ambev.Ominia.Messaging.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;
using Ambev.Ominia.Application;

namespace Ambev.Ominia.Messaging.Configurations;

public static class InfrastructureConfig
{
    public static void AddMessagingServices(this IServiceCollection services, IConfiguration configuration)
    {
        var useInMemoryBus = configuration.GetValue("USE_IN_MEMORY_BUS", false);
        
        // RabbitMQ configuration variables
        var rabbitMqUser = configuration["RABBITMQ_USER"] ?? "guest";
        var rabbitMqPassword = configuration["RABBITMQ_PASSWORD"] ?? "guest";
        var rabbitMqHost = configuration["RABBITMQ_HOST"] ?? "rabbitmq";
        var rabbitMqPort = configuration["RABBITMQ_PORT"] ?? "5672";
        var queueName = "event-queue";

        services.AddRebus(configure => configure
            .Transport(t =>
                {
                    if (useInMemoryBus)
                    {
                        t.UseInMemoryTransport(new InMemNetwork(), queueName);
                    }
                    else
                    {
                        var connectionString = configuration["Rebus:RabbitMq:ConnectionString"] ??
                                            configuration["RabbitMq:ConnectionString"] ??
                                            $"amqp://{rabbitMqUser}:{rabbitMqPassword}@{rabbitMqHost}:{rabbitMqPort}";
                        t.UseRabbitMq(connectionString, queueName);
                    }
                })
            .Routing(r => r.TypeBased().MapAssemblyOf<IEventPublisher>("event-queue"))
            .Logging(l => l.Console())
        );

        services.AutoRegisterHandlersFromAssemblyOf<IEventPublisher>();
        // Register event handlers from the Application layer
        services.AutoRegisterHandlersFromAssemblyOf<ApplicationLayer>();
        services.AddTransient<IEventPublisher, RebusEventPublisher>();
    }
}