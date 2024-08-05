
using buildingBlock.Repositories;
using buildingBlock.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Play.Catalog.Contracts;
using Play.Identity.Service.Infrastracture;
using static Duende.IdentityServer.IdentityServerConstants;

namespace Play.Identity.Service.Users.GetUserById
{
    public record GetUserByIdRequest(Guid Id);
    public record GetUserByIdResponse(UserDto UserDto);
    public class GetUserByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/users", GetUserById)
                .WithName("GetUserById")
            .WithSummary("GetUserById")
            .WithDescription("GetUserById")
            .Produces<ResponseWrapper<Guid>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
        [Authorize(Policy = LocalApi.PolicyName, Roles = Roles.Admin)]
        public static async Task<Results<Ok<ResponseWrapper<UserDto>>, BadRequest<string>>> GetUserById([AsParameters]GetUserByIdRequest request, ISender sender)
        {
            var command = request.Adapt<GetUserByIdCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<GetUserByIdResponse>();
            var responseWrapper = ResponseWrapper<UserDto>.Success(response.UserDto, "sucessfully");
            return TypedResults.Ok(responseWrapper);
        }
    }
}
