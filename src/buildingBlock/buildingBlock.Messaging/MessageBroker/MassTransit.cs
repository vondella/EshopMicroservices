
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace buildingBlock.Messaging.MessageBroker
{
  
    public static class MassTransit
    {
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
