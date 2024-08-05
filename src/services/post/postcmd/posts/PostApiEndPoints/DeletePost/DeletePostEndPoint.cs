
using Microsoft.AspNetCore.Mvc;
using postcmd.posts.PostApiEndPoints.EditPostComment;

namespace postcmd.posts.PostApiEndPoints.DeletePost
{
    public record DeletePostRequest(Guid Id, string Username);
    public record DeletePostRequestResponse(Guid Id, string Message);
    public class DeletePostEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/posts", async ([FromBody] DeletePostRequest request,ISender sender) =>
            {
                var command = request.Adapt<DeletePostCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<DeletePostRequestResponse>();
                return Results.Ok(response);
            }).WithName("Delete Post")
               .WithSummary("Delete Post")
            .WithDescription("Delete Post")
            .Produces<DeletePostRequestResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
