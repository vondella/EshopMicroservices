
using Mapster;
using MediatR;

namespace authApi.users.userRegistration
{
    public record CreateUserRequest(string Email, string Name, string PhoneNumber, string Password);
    public record UserRegistrationResponse(bool IsSuccess, string Id, string Name, string Email, string PhoneNumber, string Message);

    public class UserRegistrationEndPoint : ICarterModule

    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/register", async (CreateUserRequest request, ISender sender) =>
            {
                var command = request.Adapt<UserRegistrationCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UserRegistrationResponse>();
                return response;
            }).WithName("user registration")
               .WithSummary("user registration")
            .WithDescription("user registration")
            .Produces<UserRegistrationResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
