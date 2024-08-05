
namespace Play.Catalog.Client.Catalogs.GetCatalogs
{
    public record GetCatalogsQuery(PaginationRequest paginationRequest) : IQuery<GetCatalogsResponse>;
    public record GetCatalogsResponse(PaginatedResult<Item> Items);
    public class GetCatalogsHandler(IItemRepository<Item> _itemRepository) : IQueryHandler<GetCatalogsQuery, GetCatalogsResponse>
    {
        public async Task<GetCatalogsResponse> Handle(GetCatalogsQuery query, CancellationToken cancellationToken)
        {
            var results = await _itemRepository.GetAllAsync();
            int index = query.paginationRequest.PageIndex;
            int size = query.paginationRequest.PageSize;
            int count = 0;
            if(results != null)
            {
                results = results.Skip(query.paginationRequest.PageIndex * query.paginationRequest.PageSize).Take(query.paginationRequest.PageSize).ToList();
                count = results.Count();
            }
            var PaginatedResponseResult = new PaginatedResult<Item>(index,size,count,results);

            return new GetCatalogsResponse(PaginatedResponseResult);
        }
    }
}
