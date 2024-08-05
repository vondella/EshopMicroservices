
using Play.Inventory.Client.Inventories.GetInventoryByUserId;

namespace Play.Inventory.Client.Inventories.GrantItem
{
    public record GrantItemRequest(Guid UserId, Guid CatalogItemId, int Quantity);
    public record GrantItemResponse(bool Granted);
    public class GrantItemEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/inventory", GrantInventory)
                .WithName("GrantInventory")
            .WithSummary("GrantInventory")
            .WithDescription("GrantInventory")
            .Produces<ResponseWrapper<bool>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
        [Authorize]
        public static async Task<Results<Ok<ResponseWrapper<bool>>, BadRequest<string>>> GrantInventory(GrantItemRequest request, ISender sender)
        {
            var command = request.Adapt<GrantItemCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<GrantItemResponse>();
            var responseWrapper = ResponseWrapper<bool>.Success(response.Granted, "Inventory has granted sucessfully");
            return TypedResults.Ok(responseWrapper);
        }
    }
}
