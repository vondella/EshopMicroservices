
using buildingBlock.Responses;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Play.Trading.Purchases.GetPurchase
{
    public record GetPurchaseStateRequest(Guid CorrelationId);
    public record GetPurchaseStateResponse(PurchaseDto PurchaseDto);
    public class GetPurchaseStateEndPoint() : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/status", GetPurchaseStatus)
                .WithName("GetPurchaseStatus")
            .WithSummary("GetPurchaseStatus")
            .WithDescription("GetPurchaseStatus")
            .Produces<ResponseWrapper<PurchaseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }

        [Authorize]
        public static async Task<Results<Ok<ResponseWrapper<PurchaseDto>>, BadRequest<string>>> GetPurchaseStatus([AsParameters] GetPurchaseStateRequest request, ISender sender)
        {
            var command = request.Adapt<GetPurchaseStateCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<GetPurchaseStateResponse>();
            var responseWrapper = ResponseWrapper<PurchaseDto>.Success(response.PurchaseDto, "sucessfully");
            return TypedResults.Ok(responseWrapper);
        }
    }
}
