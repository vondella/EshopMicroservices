

namespace catalogApi.Products.GetProductById
{
    public record GetProductByIdQuery(Guid Id):IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product Product);
    public class GetProductByIdHandler(IDocumentSession session,ILogger<GetProductByIdHandler>logger) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public  async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductById Handler ");
            var product =  await session.LoadAsync<Product>(request.Id,cancellationToken);
            if(product == null)
            {
                throw new ProductNotFoundException(request.Id);
            }
            return new GetProductByIdResult(product);
        }
    }
}
