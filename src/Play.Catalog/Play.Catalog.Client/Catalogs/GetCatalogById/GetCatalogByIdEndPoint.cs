
using Play.Catalog.Client.Catalogs.UpdateCatalog;

namespace Play.Catalog.Client.Catalogs.GetCatalogById
{
    public record GetCatalogRequest(Guid Id);
    public record GetCatalogResponse(Item Item);
    public class GetCatalogByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/catalogs", GetCatalogById)
                 .WithName("get catalog by Id")
            .WithSummary("get catalog by Id")
            .WithDescription("get catalog by Id")
            .Produces<ResponseWrapper<Item>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
        [Authorize(Policies.Read)]
        public static async Task<Results<Ok<ResponseWrapper<Item>>, BadRequest<string>>> GetCatalogById([AsParameters] GetCatalogRequest request, ISender sender)
        {
            var command = request.Adapt<GetCatalogByIdQuery>();
            var result = await sender.Send(command);
            var response = result.Adapt<GetCatalogResponse>();
            var responseWrapper = ResponseWrapper<Item>.Success(response.Item, "sucessfully");
            return TypedResults.Ok(responseWrapper);
        }
    }
}
