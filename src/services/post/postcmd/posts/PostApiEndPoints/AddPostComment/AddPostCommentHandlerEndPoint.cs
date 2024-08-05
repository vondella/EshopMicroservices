
using postcmd.posts.PostApiEndPoints.EditPostMessage;

namespace postcmd.posts.PostApiEndPoints.AddPostComment
{
    public record AddPostCommentRequest(string Comment, string UserName);
    public record AddPostCommentRequestResponse(Guid Id, string Comment);
    public class AddPostCommentHandlerEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {

            app.MapPut("/posts/{Id}/comment", async (Guid Id, AddPostCommentRequest request,ISender sender) =>
            {
                var command = new AddPostCommentCommand(Id, request.Comment, request.UserName);
                var result = await sender.Send(command);
                var response = request.Adapt<AddPostCommentRequestResponse>();
                return Results.Ok(response);
            }).WithName("Add Post Comment")
               .WithSummary("Add Post Comment")
            .WithDescription("Add Post Comment")
            .Produces<AddPostCommentRequestResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
