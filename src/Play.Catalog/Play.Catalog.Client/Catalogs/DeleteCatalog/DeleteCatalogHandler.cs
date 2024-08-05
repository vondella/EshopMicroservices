
using MassTransit;
using MassTransit.Transports;
using Play.Catalog.Contracts;

namespace Play.Catalog.Client.Catalogs.DeleteCatalog
{
    public record DeleteCatalogRequest(Guid Id):ICommand<DeleteCatalogResult>;
    public record DeleteCatalogResult(Guid Id);
    public class DeleteCatalogHandler(IItemRepository<Item> _itemRepository,IPublishEndpoint _publishEndpoint) : ICommandHandler<DeleteCatalogRequest, DeleteCatalogResult>
    {
        public async Task<DeleteCatalogResult> Handle(DeleteCatalogRequest request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetAsync(request.Id);
            if(item == null)
            {
                throw new NotFoundException($"item with Id {request.Id} is not found");
            }
            await _itemRepository.RemoveAsync(request.Id);
            await _publishEndpoint.Publish(new CatalogItemDeleted(request.Id));
            return new DeleteCatalogResult(request.Id);
        }
    }
}
