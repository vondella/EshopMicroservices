﻿using MassTransit;
using System.Reflection;

namespace postQuery
{
    public static class DependencuInjection
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
                });
            });

            return services;
        }
    }
}
