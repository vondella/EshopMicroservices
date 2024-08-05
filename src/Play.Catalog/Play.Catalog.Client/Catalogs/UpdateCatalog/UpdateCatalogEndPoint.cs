
namespace Play.Catalog.Client.Catalogs.UpdateCatalog
{
    public record UpdateCatalogRequest(Guid Id, string Name, string Description, decimal Price);
    public record UpdateCatalogResponse(Guid Id);
    public class UpdateCatalogEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/catalogs", EditCatalog)
                  .WithName("update catalogs")
            .WithSummary("update catalogs")
            .WithDescription("update catalogs")
            .Produces<ResponseWrapper<Guid>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
        [Authorize(Policies.Write)]
        public static async Task<Results<Ok<ResponseWrapper<Guid>>, BadRequest<string>>> EditCatalog(UpdateCatalogRequest request, ISender sender)
        {
            var command = request.Adapt<UpdateCatalogCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<UpdateCatalogResponse>();
            var responseWrapper = ResponseWrapper<Guid>.Success(response.Id, "sucessfully updated");
            return TypedResults.Ok(responseWrapper);
        }
    }
}
