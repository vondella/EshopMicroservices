
using Play.Identity.Service.Infrastracture;
using Play.Identity.Service.Users.GetUserById;
using Play.Identity.Service.Users.GetUsers;
using static Duende.IdentityServer.IdentityServerConstants;

namespace Play.Identity.Service.Users.UpdateUser
{
    public record UpdateUserRequest(Guid Id, string Email, decimal Gil);
    public record UpdateUserResponse(bool Succeded);
    public class UpdateUserEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/editUser", UpdateUser)
                .WithName("UpdateUser")
            .WithSummary("UpdateUser")
            .WithDescription("UpdateUser")
            .Produces<ResponseWrapper<bool>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
        [Authorize(Policy = LocalApi.PolicyName, Roles = Roles.Admin)]
        public static async Task<Results<Ok<ResponseWrapper<bool>>, BadRequest<string>>> UpdateUser(UpdateUserRequest request, ISender sender)
        {
            var command = request.Adapt<UpdateUserCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<UpdateUserResponse>();
            var responseWrapper = ResponseWrapper<bool>.Success(response.Succeded, "user has been edited sucessfully");
            return TypedResults.Ok(responseWrapper);
        }
    }
}
