
namespace Play.Inventory.Client.Inventories.GetInventoryByUserId
{
    public record GetInventoryByUserRequest(Guid UserId);
    public record GetInventoryByIdResponse(IEnumerable<InventoryItemDto> CatalogItems);
    public class GetInventoryByUserIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/inventoryByUserId", GetInventoryByUserId)
             .WithName("inventoryByUserId")
            .WithSummary("inventoryByUserId")
            .WithDescription("inventoryByUserId")
            .Produces<ResponseWrapper<IEnumerable<InventoryItemDto>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }

        [Authorize]
        public static async Task<Results<Ok<ResponseWrapper<IEnumerable<InventoryItemDto>>>, BadRequest<string>>> GetInventoryByUserId([AsParameters]GetInventoryByUserRequest request, ISender sender)
        {
            var command = request.Adapt<GetInventoryByIdQuery>();
            var result = await sender.Send(command);
            var response = result.Adapt<GetInventoryByIdResult>();
            var responseWrapper = ResponseWrapper<IEnumerable<InventoryItemDto>>.Success(response.CatalogItems, "sucessfully");
            return TypedResults.Ok(responseWrapper);
        }
    }
}
