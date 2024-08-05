
using buildingBlock.Entities;
using Play.Identity.Contract;
using Play.Inventory.Contracts;
using Play.Trading.Exceptions;
using Play.Trading.Settings;

namespace Play.Trading
{
    public static  class DependencyInjection
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
        public static IServiceCollection AddTradingAuthentication(this IServiceCollection services)
        {

            services.ConfigureOptions<ConfigureJwtBearerOptions>()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();
            return services;
        }
        public static IServiceCollection AddMessageBroker(this IServiceCollection services,
       IConfiguration configuration,MongoConfig mongoConfig,
       Assembly? assembly = null)
        {
            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();

                if (assembly != null)
                    config.AddConsumers(assembly);

                config.AddSagaStateMachine<PurchaseStateMachine, PurchaseState>(sagaConfigurator =>
                {
                    sagaConfigurator.UseInMemoryOutbox();
                })
                .MongoDbRepository(r =>
                {
                    r.Connection = mongoConfig.ConnectionString;
                    r.DatabaseName = mongoConfig.Database;
                });
                config.UsingPlayEconomyRabbitMq(configuration, retryConfigurator =>
                {
                    retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
                    retryConfigurator.Ignore(typeof(UnknownItemException));
                });
            
            });
            var queueSettings = configuration.GetSection(nameof(QueueSettings))
                                                       .Get<QueueSettings>();
            EndpointConvention.Map<GrantItems>(new Uri(queueSettings.GrantItemsQueueAddress));
            EndpointConvention.Map<DebitGil>(new Uri(queueSettings.DebitGilQueueAddress));
            EndpointConvention.Map<SubtractItems>(new Uri(queueSettings.SubtractItemsQueueAddress));
          
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
            config.AddRequestClient<GetPurchaseState>();
        }

    }
}
