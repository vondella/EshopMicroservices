
using postcmd.posts.PostApiEndPoints.AddPost;

namespace postcmd.posts.PostApiEndPoints.EditPostComment
{
    public record EditCommentRequest(string Comment, string Username);
    public record EditResponse(Guid Id, string Message);
    public class EditMessageEndPoint: ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/posts/{Id}/comment/{CommentId}", async (Guid Id,Guid CommentId, EditCommentRequest request,ISender sender) =>
            {
                var command=new EditCommentCommand(Id:Id, CommentId:CommentId,request.Comment,request.Username);
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
