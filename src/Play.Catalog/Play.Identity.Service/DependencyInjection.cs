

using MassTransit;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;

namespace Play.Identity.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityMongo(this IServiceCollection services,IConfiguration configuration)
        {
            var connectionstring = configuration["MongoConfig:ConnectionString"];
            var databasename = configuration["MongoConfig:Database"];
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            services.AddDefaultIdentity<ApplicationUser>()
                 .AddRoles<ApplicationRole>()
                 .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(configuration["MongoConfig:ConnectionString"],
                   configuration["MongoConfig:Database"]);
            return services;
        }
        public static IServiceCollection AddUserIdentityServer(this IServiceCollection services,IdentityServerSettings settings)
        {
            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
                .AddAspNetIdentity<ApplicationUser>()
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryClients(Config.Clients)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddDeveloperSigningCredential();
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
