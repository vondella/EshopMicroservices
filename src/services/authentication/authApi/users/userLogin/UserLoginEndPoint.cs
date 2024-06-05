

using Mapster;

namespace authApi.users.userLogin
{
    public record LoginUserRequest(string Username, string Password);
    public record LoginUserResponse(bool IsSuccess, string Message, string Id, string Name, string Email, string PhoneNumber, string Token);

    public class UserLoginEndPoint : ICarterModule

    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/login", async (LoginUserRequest request, ISender sender) =>
            {
                var command = request.Adapt<LoginUserCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<LoginUserResponse>();
                return  response;
            }).WithName("login")
               .WithSummary("user login")
            .WithDescription("user login")
            .Produces<LoginUserResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
