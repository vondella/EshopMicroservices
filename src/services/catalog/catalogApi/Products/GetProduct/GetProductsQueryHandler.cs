
using Marten.Pagination;

namespace catalogApi.Products.GetProduct
{
    public record GetProductQuery (int? PageNumber, int? PageSize) : IQuery<GetProductResult>;
    public record GetProductResult(IEnumerable<Product> Products);
    public class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler>logger) : IQueryHandler<GetProductQuery, GetProductResult>
    {
        public async Task<GetProductResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetQuery handle products");
            var products = await session.Query<Product>().ToPagedListAsync(request.PageNumber?? 1,request.PageSize?? 10,cancellationToken);
            return new GetProductResult(products);
        }
    }
}
