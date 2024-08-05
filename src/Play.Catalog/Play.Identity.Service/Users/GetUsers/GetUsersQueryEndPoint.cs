
using static Duende.IdentityServer.IdentityServerConstants;
namespace Play.Identity.Service.Users.GetUsers
{
    public record GetUsersResponse(PaginatedResult<UserDto> Users);

    public class GetUsersQueryEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/usersAll", GetAllUsers)
            .WithName("GetAllUsers")
            .WithSummary("GetAllUsers")
            .WithDescription("GetAllUsers")
            .Produces<ResponseWrapper<PaginatedResult<UserDto>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest); 
        }
        [Authorize(Policy=LocalApi.PolicyName,Roles =AdminConstants.AdminRole)]
        public static async Task<Results<Ok<ResponseWrapper<PaginatedResult<UserDto>>>, BadRequest<string>>> GetAllUsers([AsParameters] PaginationRequest request, ISender sender)
        {
            var result = await sender.Send(new GetUsersQuery(request));
            var response = result.Adapt<GetUsersResponse>();
            var responseWrapper = ResponseWrapper<PaginatedResult<UserDto>>.Success(response.Users, "sucessfully");
            return TypedResults.Ok(responseWrapper);
        }
    }
}
