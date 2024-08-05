using buildingBlock.Identity;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;

namespace Inventory.Grpc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services,
       IConfiguration configuration,
       Assembly? assembly = null)
        {
            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();

                if (assembly != null)
                    config.AddConsumers(assembly);

                config.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                    {
                        host.Username(configuration["MessageBroker:UserName"]);
                        host.Password(configuration["MessageBroker:Password"]);
                    });
                    configurator.ConfigureEndpoints(context);
                    configurator.UseMessageRetry(retryConfigurator =>
                    {
                        retryConfigurator.Interval(5, TimeSpan.FromSeconds(10));
                    });
                });
            });

            return services;
        }
        public static IServiceCollection AddInventoryAUthentication(this IServiceCollection services)
        {

            services.ConfigureOptions<ConfigureJwtBearerOptions>()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();
            return services;
        }
    }

}
