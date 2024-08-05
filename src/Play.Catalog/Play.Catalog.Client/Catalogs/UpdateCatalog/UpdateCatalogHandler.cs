using buildingBlock.Exceptions;
using buildingBlock.Repositories;
using MassTransit;
using MassTransit.Transports;
using Play.Catalog.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Client.Catalogs.UpdateCatalog
{
    public record UpdateCatalogCommand(Guid Id,string Name, string Description, decimal Price):ICommand<UpdateCatalogResult>;
    public record UpdateCatalogResult(Guid Id);
    public class UpdateCatalogHandler(IItemRepository<Item> _itemRepository, IPublishEndpoint _publishEndPoint) : ICommandHandler<UpdateCatalogCommand, UpdateCatalogResult>
    {
        public async  Task<UpdateCatalogResult> Handle(UpdateCatalogCommand command, CancellationToken cancellationToken)
        {
            var existingItem = await _itemRepository.GetAsync(command.Id);
            if(existingItem == null)
            {
                throw new NotFoundException($"item with id {command.Id} doesnot exist");
            }

            existingItem.Name = command.Name;
            existingItem.Description = command.Description;
            existingItem.Price = command.Price;
            await _itemRepository.UpdateAsync(existingItem);

            await _publishEndPoint.Publish(new CatalogItemUpdated(
               existingItem.Id,
               existingItem.Name,
               existingItem.Description,
               existingItem.Price));

            return new UpdateCatalogResult(command.Id);
        }
    }
}
