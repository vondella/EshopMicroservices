using ordering.application.Orders.Commands.DeleteOrder;namespace OrderingApi.EndPoints;public record DeleteOrderRequest(Guid Id);public record DeleteOrderResponse(bool IsSuccess);public class DeleteOrder : ICarterModule{	public void AddRoutes(IEndpointRouteBuilder app)	{		app.MapDelete("/orders/{id}", async (Guid id, ISender sender) =>
		{
			var result = await sender.Send(new DeleteOrderCommand(id));
			var response = result.Adapt<DeleteOrderResponse>();
			return Results.Ok(response);
		}).WithName("delete Order")
         .Produces<DeleteOrderResponse>(StatusCodes.Status201Created)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithSummary("delete Order")
         .WithDescription("delete Order");
    }}