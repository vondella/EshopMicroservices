
using postcmd.posts.PostApiEndPoints.AddPost;

namespace postcmd.posts.PostApiEndPoints.EditPostComment
{
    public record EditCommentRequest(Guid Id, Guid CommentId, string Comment, string Username);
    public record EditResponse(Guid Id, string Message);
    public class EditMessageEndPoint: ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/posts/comment", async ( EditCommentRequest request,ISender sender) =>
            {
                var command=request.Adapt<EditCommentCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<EditResponse>();
                return Results.Ok(response);

            }).WithName("Edit Post comment")
               .WithSummary("Edit Post Message")
            .WithDescription("Edit Post Message")
            .Produces<EditResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
