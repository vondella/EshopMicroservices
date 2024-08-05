using buildingBlock.Repositories;
using Inventory.Grpc.Domain;
using MassTransit;
using Play.Catalog.Contracts;

namespace Inventory.Grpc.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        private readonly IItemRepository<CatalogItem> _itemRepository;

        public CatalogItemUpdatedConsumer(IItemRepository<CatalogItem> itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;
            var item = await _itemRepository.GetAsync(message.ItemId);
            if (item == null)
            {
                item = new CatalogItem
                {
                    Name = message.Name,
                    Description = message.Description,
                    Id = message.ItemId
                };
                await _itemRepository.CreateAsync(item);
            }
            else
            {
                item.Name = message.Name;
                item.Description = message.Description;
                await _itemRepository.UpdateAsync(item);
            }        
        }
    }
}
