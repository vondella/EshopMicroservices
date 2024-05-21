using Marten.Schema;

namespace catalogApi.Data
{
    public class CatalogInitialData : IInitialData
    {
        public  async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();
            if (await session.Query<Product>().AnyAsync())
                return;

            session.Store<Product>(GetPreconfiguredProducts());
            await session.SaveChangesAsync();
        }

        private IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product{ Id=new Guid(), Description="This phone is for company",
                    Name="IPhone", price=20000, ImageFile="no image", Category=new List<string>{"smart phone"}}
            };
        }
    }
}
