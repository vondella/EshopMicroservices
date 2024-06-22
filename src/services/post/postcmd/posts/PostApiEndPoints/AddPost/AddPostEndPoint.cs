
namespace postcmd.posts.PostApiEndPoints.AddPost
{
    public record AddPostRequest( string Author, string Message) ;
    public record AddPostRequestResponse(Guid Id, string Message);
    public class AddPostEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/posts", async (AddPostRequest request, ISender sender) =>
            {
                var command = request.Adapt<AddPostCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<AddPostRequestResponse>();
                return Results.Created("", response);
            }).WithName("Add Post")
               .WithSummary("Add Post")
            .WithDescription("Add Post")
            .Produces<AddPostRequestResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
