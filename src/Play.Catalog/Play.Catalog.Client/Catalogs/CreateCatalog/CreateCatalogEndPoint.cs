
namespace Play.Catalog.Client.Catalogs.CreateCatalog
{
    public record CreateItemCatalogRequest(string Name, decimal Price, string Description);
    public class CreateCatalogEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/catalogs", PostCatalog)
                  .WithName("create catalogs")
            .WithSummary("create catalogs")
            .WithDescription("create catalogs")
            .Produces<ResponseWrapper<Guid>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest); 
        }

        [Authorize(Policies.Write)]
        public static async Task<Results<Ok<ResponseWrapper<Guid>>, BadRequest<string>>> PostCatalog(CreateItemCatalogRequest request, ISender sender)
        {
            var command = request.Adapt<CreateItemCatalogCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateItemCataloResponse>();
            var responseWrapper = ResponseWrapper<Guid>.Success(response.Id, "sucessfully");
            return TypedResults.Ok(responseWrapper);
        }
    }
}
