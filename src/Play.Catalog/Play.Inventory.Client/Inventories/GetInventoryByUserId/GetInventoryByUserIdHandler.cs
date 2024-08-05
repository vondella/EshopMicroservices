
using Play.Inventory.Client.Extensions;
using System.Security.Claims;

namespace Play.Inventory.Client.Inventories.GetInventoryByUserId
{
    public record GetInventoryByIdQuery(Guid UserId):IQuery<GetInventoryByIdResult>;
    public record GetInventoryByIdResult(IEnumerable<InventoryItemDto> CatalogItems);
    public class GetInventoryByUserIdHandler(IItemRepository<InventoryItem> _inventoryItemRepository, IItemRepository<CatalogItem> _inventoryCatalogItemRepository,
        IHttpContextAccessor _httpAccessor) : IQueryHandler<GetInventoryByIdQuery, GetInventoryByIdResult>
    {
        public async  Task<GetInventoryByIdResult> Handle(GetInventoryByIdQuery command, CancellationToken cancellationToken)
        {
            var inventoryItemEntities = await _inventoryItemRepository.GetAllAsync(item => item.UserId == command.UserId);
            if (inventoryItemEntities == null)
                throw new NotFoundException($"User with Id {command.UserId} has no inventory");

            var currentUserId = _httpAccessor.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (Guid.Parse(currentUserId) != command.UserId)
            {
                if (!_httpAccessor.HttpContext.User.IsInRole("Admin"))
                {
                     throw new NotFoundException($"no permission for this");
                }
            }

            var itemIds = inventoryItemEntities.Select(item => item.CatalogItemId);

            var catalogItemEntities = await _inventoryCatalogItemRepository.GetAllAsync(item => itemIds.Contains(item.Id));

            var inventoryItemDtos = inventoryItemEntities.Select(inventoryItem =>
            {
                var catalogItem = catalogItemEntities.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
                return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
            });
            return new GetInventoryByIdResult(inventoryItemDtos);
        }
    }
}
