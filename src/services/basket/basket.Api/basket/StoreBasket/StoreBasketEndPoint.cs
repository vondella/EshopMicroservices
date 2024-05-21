

using basketApi.basket.GetBasket;

namespace basketApi.basket.StoreBasket
{ 
    public record StoreBasketRequest(ShoppingCart Cart);
    public record StoreBasketRespone(string userName);
    public class StoreBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<StoreBasketCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<StoreBasketRespone>();
                return Results.Created($"/basket/{response.userName}", response);

            }).WithName("Store Shopping Cart")
            .WithSummary("Store Shopping Cart")
            .WithDescription("Store Shopping Cart")
            .Produces<StoreBasketRespone>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
