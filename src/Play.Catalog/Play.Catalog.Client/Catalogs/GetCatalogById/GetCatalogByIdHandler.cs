
namespace Play.Catalog.Client.Catalogs.GetCatalogById
{
    public record GetCatalogByIdQuery(Guid Id):IQuery<GetCatalogByIdResult>;
    public record GetCatalogByIdResult(Item Item);
    public class GetCatalogByIdHandler(IItemRepository<Item> _itemRepository) : IQueryHandler<GetCatalogByIdQuery, GetCatalogByIdResult>
    {
        public async Task<GetCatalogByIdResult> Handle(GetCatalogByIdQuery query, CancellationToken cancellationToken)
        {
            var result = await _itemRepository.GetAsync(query.Id);
            if (result == null)
            {
                throw new NotFoundException($"catalog with Id {query.Id} is  not found ");
            }
            return new GetCatalogByIdResult(result);
        }
    }
}
