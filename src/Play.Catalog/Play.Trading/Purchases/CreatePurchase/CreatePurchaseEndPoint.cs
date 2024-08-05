
using buildingBlock.CQRS;
using buildingBlock.Responses;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Play.Trading.Purchases.GetPurchase;

namespace Play.Trading.Purchases.CreatePurchase
{
    public record CreatePurchaseRequest(Guid ItemId, int Quantity, Guid IdempotencyId);
    public record CreatePurchaseResponse(Guid IdempotencyId);
    public class CreatePurchaseEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/purchase", CreatePurchaseRequest)
                     .WithName("CreatePurchaseRequest")
            .WithSummary("CreatePurchaseRequest")
            .WithDescription("CreatePurchaseRequest")
            .Produces<ResponseWrapper<Guid>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
        [Authorize]
        public static async Task<Results<Ok<ResponseWrapper<Guid>>, BadRequest<string>>> CreatePurchaseRequest(CreatePurchaseRequest request, ISender sender)
        {
            var command = request.Adapt<CreatePurchaseCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreatePurchaseResponse>();
            var responseWrapper = ResponseWrapper<Guid>.Success(response.IdempotencyId, "sucessfully");
            return TypedResults.Ok(responseWrapper);
        }
    }
}
