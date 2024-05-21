

namespace catalogApi.Products.GetProduct
{
    public record GetProductRequest(int? PageNumber,int? PageSize);
    public record GetProductsResponse(IEnumerable<Product> Products);
    public class GetProductsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] GetProductRequest request,ISender sender) =>
            {
                var query = request.Adapt<GetProductQuery>();

                var result = await sender.Send(query);              
                var response = result.Adapt<GetProductsResponse>();             
                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .WithSummary("Get products")
            .WithDescription("Get products")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
