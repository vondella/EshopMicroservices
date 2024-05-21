﻿
using ordering.application.Orders.Commands.UpdateOrder;

namespace OrderingApi.EndPoints;
public record UpdateOrderRequest(OrderDto Order);
public record UpdateOrderResponse(bool IsSuccess);

public class UpdateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/orders", async (UpdateOrderRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateOrderCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<UpdateOrderResponse>();
            return Results.Ok(response);
        })
          .WithName("Update Order")
         .Produces<UpdateOrderResponse>(StatusCodes.Status201Created)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithSummary("update Order")
         .WithDescription("update Order");
    }
}

