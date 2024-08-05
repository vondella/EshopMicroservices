
namespace Play.Trading.Consumers
{
    public class TradingCatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IItemRepository<CatalogItem> repository;

        public TradingCatalogItemDeletedConsumer(IItemRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }

        public async  Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            var message = context.Message;

            var item = await repository.GetAsync(message.ItemId);

            if (item == null)
            {
                return;
            }

            await repository.RemoveAsync(message.ItemId);
        }
    }
}
