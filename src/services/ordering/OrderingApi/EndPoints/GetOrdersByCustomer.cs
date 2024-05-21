
using ordering.application.Orders.Queries.GetOrdersByCustomer;

namespace OrderingApi.EndPoints;
public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);
public class GetOrdersByCustomer : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
         app.MapGet("/orders/customer/{customerId}",async (Guid customerId,ISender sender) =>
         {
             var result = await sender.Send(new GetOrderByCustomerQuery(customerId));
             var response = result.Adapt<GetOrdersByCustomerResponse>();
             return Results.Ok(response);
         })
          .WithName("get Order by  customer")
         .Produces<GetOrdersByNameResponse>(StatusCodes.Status201Created)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithSummary("get Order by  customer")
         .WithDescription("get Order by  customer");
    }
}
