using buildingBlock.Repositories;
using Inventory.Grpc.Domain;
using MassTransit;
using Play.Catalog.Contracts;

namespace Inventory.Grpc.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly IItemRepository<CatalogItem> _itemRepository;

        public CatalogItemCreatedConsumer(IItemRepository<CatalogItem> itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;
            var item = await _itemRepository.GetAsync(message.ItemId);
            if (item != null) return;

            item = new CatalogItem
            {
                Name = message.Name,
                Description = message.Description,
                Id = message.ItemId
            };
            await _itemRepository.CreateAsync(item);
        }
    }
}
