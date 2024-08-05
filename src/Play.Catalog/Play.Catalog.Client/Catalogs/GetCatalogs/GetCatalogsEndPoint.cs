
namespace Play.Catalog.Client.Catalogs.GetCatalogs
{
    public record GetOrdersResponse(PaginatedResult<Item> Items);

    public class GetCatalogsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/items", GetAllCatalogs)
                .WithName("items")
            .WithSummary("items")
            .WithDescription("items")
            .Produces<ResponseWrapper<PaginatedResult<Item>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        }

        [Authorize(Policies.Read)]
        public static async Task<Results<Ok<ResponseWrapper<PaginatedResult<Item>>>, BadRequest<string>>> GetAllCatalogs([AsParameters] PaginationRequest query, ISender sender)
        {

            var result = await sender.Send(new GetCatalogsQuery(query));
            var response = result.Adapt<GetCatalogsResponse>();
            var responseWrapper = ResponseWrapper<PaginatedResult<Item>>.Success(response.Items, "sucessfully");
            return TypedResults.Ok(responseWrapper);
        }
    }
}
