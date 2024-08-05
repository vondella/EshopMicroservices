
namespace Play.Trading.Consumers
{
    public class TradingCatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly IItemRepository<CatalogItem> repository;

        public TradingCatalogItemCreatedConsumer(IItemRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;

            var item = await repository.GetAsync(message.ItemId);

            if (item != null)
            {
                return;
            }

            item = new CatalogItem
            {
                Id = message.ItemId,
                Name = message.Name,
                Description = message.Description,
                Price = message.Price
            };

            await repository.CreateAsync(item);
        }
    }
}
