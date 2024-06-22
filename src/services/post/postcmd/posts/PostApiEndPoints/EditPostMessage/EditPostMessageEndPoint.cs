
using postcmd.posts.PostApiEndPoints.EditPostComment;

namespace postcmd.posts.PostApiEndPoints.EditPostMessage
{
    public record EditPostMessageRequest(Guid Id, string Message);
    public record EditPostMessageRequestResponse(Guid Id,string Message);
    public class EditPostMessageEndPoint: ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/posts",async (EditPostMessageRequest request,ISender sender) =>
            {
                var command = request.Adapt<EditPostMessageCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<EditPostMessageRequestResponse>();
                return Results.Ok(response);
            }).WithName("Edit Post Message")
               .WithSummary("Edit Post Message")
            .WithDescription("Edit Post Message")
            .Produces<EditPostMessageRequestResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
