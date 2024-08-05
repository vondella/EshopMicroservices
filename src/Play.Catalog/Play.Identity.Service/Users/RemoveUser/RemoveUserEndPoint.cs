
using Play.Identity.Service.Infrastracture;
using Play.Identity.Service.Users.UpdateUser;
using static Duende.IdentityServer.IdentityServerConstants;

namespace Play.Identity.Service.Users.RemoveUser
{
    public record RemoveUserRequest(Guid Id);
    public record RemoveUserResponse(bool Deleted);
    public class RemoveUserEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/removeUser", RemoveUserAsync)
                      .WithName("RemoveUserAsync")
            .WithSummary("RemoveUserAsync")
            .WithDescription("RemoveUserAsync")
            .Produces<ResponseWrapper<bool>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
        [Authorize(Policy = LocalApi.PolicyName, Roles = Roles.Admin)]
        public static async Task<Results<Ok<ResponseWrapper<bool>>, BadRequest<string>>> RemoveUserAsync([AsParameters]RemoveUserRequest request, ISender sender)
        {
            var command = request.Adapt<RemoveUserCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<RemoveUserResponse>();
            var responseWrapper = ResponseWrapper<bool>.Success(response.Deleted, "user has been deleted sucessfully");
            return TypedResults.Ok(responseWrapper);
        }
    }
}
