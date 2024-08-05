
using Microsoft.AspNetCore.Mvc;
using postcmd.posts.PostApiEndPoints.LikePost;

namespace postcmd.posts.PostApiEndPoints.RemovePostComment
{
    public record RemoveCommentRequest(Guid CommentId, string Username);
    public record RemoveCommentRequestResponse(Guid CommentId, string Message);
    public class RemoveCommentHandlerEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/posts/{Id}/comment", async (Guid Id,[FromBody]RemoveCommentRequest request,ISender sender) =>
            {
                var command = new RemoveCommentCommand(Id, request.CommentId, request.Username);
                var result = await sender.Send(command);
                var response = result.Adapt<RemoveCommentRequestResponse>();
                return Results.Ok(response);
            }).WithName("Remove Comment")
               .WithSummary("Remove Comment")
            .WithDescription("Remove Comment")
            .Produces<RemoveCommentRequestResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
