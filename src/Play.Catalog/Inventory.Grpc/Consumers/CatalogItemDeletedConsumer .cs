using buildingBlock.Repositories;
using Inventory.Grpc.Domain;
using MassTransit;
using Play.Catalog.Contracts;

namespace Inventory.Grpc.Consumers
{
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IItemRepository<CatalogItem> _itemRepository;

        public CatalogItemDeletedConsumer(IItemRepository<CatalogItem> itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            var message = context.Message;
            var item = await _itemRepository.GetAsync(message.ItemId);
            if (item == null) return;

       
            await _itemRepository.RemoveAsync(item.Id);
        }
    }
}
