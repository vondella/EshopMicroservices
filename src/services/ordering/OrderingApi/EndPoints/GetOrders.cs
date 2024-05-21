using buildingBlock.Pagination;
using ordering.application.Orders.Queries.GetOrders;

namespace OrderingApi.EndPoints;
public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);
public class GetOrders : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", async ([AsParameters] PaginationRequest request, ISender sender) =>
        {
            var result = await sender.Send(new GetOrdersQuery(request));
            var response = result.Adapt<GetOrdersResponse>();
            return Results.Ok(response);
        })
          .WithName("get Orders")
         .Produces<GetOrdersResponse>(StatusCodes.Status201Created)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithSummary("get Orders")
         .WithDescription("get Orders");
    }
}
