
using Play.Catalog.Client.Catalogs.GetCatalogById;

namespace Play.Catalog.Client.Catalogs.DeleteCatalog
{
    public record DeleteRequest(Guid Id);
    public record DeleteResponse(Guid Id);
    public class DeleteCatalogEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/catalogs", DeleteCatalogById)
                .WithName("delete catalog by Id")
            .WithSummary("delete catalog by Id")
            .WithDescription("delete catalog by Id")
            .Produces<ResponseWrapper<Guid>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
        [Authorize(Policies.Write)]
        public static async Task<Results<Ok<ResponseWrapper<Guid>>, BadRequest<string>>> DeleteCatalogById([AsParameters] DeleteRequest request, ISender sender)
        {
            var command = request.Adapt<DeleteCatalogRequest>();
            var result = await sender.Send(command);
            var response = result.Adapt<DeleteCatalogResult>();
            var responseWrapper = ResponseWrapper<Guid>.Success(response.Id, "sucessfully deleted");
            return TypedResults.Ok(responseWrapper);
        }
    }
}
