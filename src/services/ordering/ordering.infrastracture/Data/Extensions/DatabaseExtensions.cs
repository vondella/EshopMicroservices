

using Microsoft.AspNetCore.Builder;

namespace ordering.infrastracture.Data.Extensions
{
    public static   class DatabaseExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.MigrateAsync().GetAwaiter().GetResult();
            await SeedAsync(context);
        }

        private static  async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedCustomerAsync(context);
            await SeedProductAsync(context);
            await SeedOrderItemAsync(context);
        }

        private static async Task SeedCustomerAsync(ApplicationDbContext context)
        {
             if(!context.Customers.Any())
            {
                await context.Customers.AddRangeAsync(InitialData.Customers);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedProductAsync(ApplicationDbContext context)
        {

            if (!context.Products.Any())
            {
                await context.Products.AddRangeAsync(InitialData.Products);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedOrderItemAsync(ApplicationDbContext context)
        {
            if (!context.Orders.Any())
            {
                await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
                await context.SaveChangesAsync();
            }
        }

    }
}
