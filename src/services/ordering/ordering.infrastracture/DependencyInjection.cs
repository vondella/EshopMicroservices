


using ordering.application.Data;

namespace ordering.infrastracture
{
    public static  class DependencyInjection
    {
        public static IServiceCollection  AddInfrastractureServices(this IServiceCollection services,IConfiguration configuration )
        {
            var connectionstring = configuration.GetConnectionString("Database");
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptors>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp,options)=>
            {
                options.AddInterceptors(sp.GetService<ISaveChangesInterceptor>());
                options.UseSqlServer(connectionstring);
            }
            );
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            return services;
        }
    }
}
