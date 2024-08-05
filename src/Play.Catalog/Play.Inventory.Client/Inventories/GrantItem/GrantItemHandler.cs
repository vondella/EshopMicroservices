
using Play.Inventory.Contracts;

namespace Play.Inventory.Client.Inventories.GrantItem
{
    public record GrantItemCommand(Guid UserId, Guid CatalogItemId, int Quantity) :ICommand<GrantItemResult>;
    public record GrantItemResult(bool Granted);
    public class GrantItemHandler(IItemRepository<InventoryItem> _inventoryItemRepository,IPublishEndpoint _publishEndPoint) : ICommandHandler<GrantItemCommand, GrantItemResult>
    {
        public async  Task<GrantItemResult> Handle(GrantItemCommand command, CancellationToken cancellationToken)
        {
            var inventoryItem = await _inventoryItemRepository.GetAsync(
              item => item.UserId == command.UserId && item.CatalogItemId == command.CatalogItemId);

            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem
                {
                    CatalogItemId = command.CatalogItemId,
                    UserId = command.UserId,
                    Quantity = command.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };

                await _inventoryItemRepository.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += command.Quantity;
                await _inventoryItemRepository.UpdateAsync(inventoryItem);
            }
            await _publishEndPoint.Publish(new InventoryItemUpdated(
           inventoryItem.UserId,
           inventoryItem.CatalogItemId,
           inventoryItem.Quantity
           ));

            return new GrantItemResult(true);
        }
    }
}
