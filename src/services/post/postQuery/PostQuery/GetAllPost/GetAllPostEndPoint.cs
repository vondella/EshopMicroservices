

using Mapster;

namespace postQuery.PostQuery.GetAllPost
{
    public record GetAllPostRequest(int? PageNumber, int? PageSize);
    public record GetAllPostResponse(IEnumerable<PostEntity> Posts);
    public class GetAllPostEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/posts", async ([AsParameters] GetAllPostRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetAllPostQuery>();
                var result = sender.Send(query);
                var response = result.Adapt<GetAllPostResponse>();
                return Results.Ok(response);
            }).WithName("GetAllPosts")
            .WithSummary("GetAllPosts")
            .WithDescription("GetAllPosts")
            .Produces<GetAllPostResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
