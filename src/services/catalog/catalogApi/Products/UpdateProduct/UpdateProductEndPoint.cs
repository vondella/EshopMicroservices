

namespace catalogApi.Products.UpdateProduct
{
    public record UpdateProductRequest(Guid Id, string Name, List<string> Category,
      string Description, string ImageFile, decimal price) : ICommand<UpdateProductResponse>;
    public record UpdateProductResponse(bool IsSuccess);

    public  class UpdateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateProductResponse>();
                return Results.Ok(response);
            })
             .WithName("PutProducts")
            .WithSummary("Put products ")
            .WithDescription("Put products")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
