using buildingBlock.Config;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using buildingBlock.Entities;
using buildingBlock.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;

namespace Play.Inventory.Client
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMongoSettings(this IServiceCollection services, IConfiguration configuration)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
            services.Configure<MongoConfig>(configuration.GetSection(nameof(MongoConfig)));
            return services;
        }
        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services) where T : IEntity
        {
            services.AddScoped<IItemRepository<T>, ItemRepository<T>>();
            return services;
        }
        public static IServiceCollection AddInventoryAuthentication(this IServiceCollection services)
        {
            services.ConfigureOptions<ConfigureJwtBearerOptions>()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();
            return services;
        }
        public static IServiceCollection AddMessageBroker(this IServiceCollection services,
IConfiguration configuration,
Assembly? assembly = null, Action<IRetryConfigurator> configuratorEntries = null)
        {
            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();

                if (assembly != null)
                    config.AddConsumers(assembly);

                config.UsingPlayEconomyRabbitMq(configuration, configuratorEntries);
            });

            return services;
        }
        public static void UsingPlayEconomyRabbitMq(this IBusRegistrationConfigurator config, IConfiguration configuration, Action<IRetryConfigurator> configureRetries = null)
        {
            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:UserName"]);
                    host.Password(configuration["MessageBroker:Password"]);
                });
                configurator.ConfigureEndpoints(context);

                if (configureRetries == null)
                {
                    configureRetries = (retryConfigurator) => retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
                }
                configurator.UseMessageRetry(configureRetries);
            });
        }
    }
}
