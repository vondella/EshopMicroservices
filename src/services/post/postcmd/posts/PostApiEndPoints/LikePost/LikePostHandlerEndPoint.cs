
using postcmd.posts.PostApiEndPoints.EditPostMessage;

namespace postcmd.posts.PostApiEndPoints.LikePost
{
    public record LikePostRequest(Guid Id);
    public record LikePostRequestResponse(Guid Id, string Message);
    public class LikePostHandlerEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/posts/like", async (LikePostRequest request,ISender sender) =>
            {
                var command = request.Adapt<LikePostCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<LikePostRequestResponse>();
                return Results.Ok(response);
            }).WithName("Like Post")
               .WithSummary("Like Post")
            .WithDescription("Like Post")
            .Produces<LikePostRequestResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
