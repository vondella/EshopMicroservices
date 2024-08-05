

using buildingBlock.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Play.Identity.Service.Users.GetUserById
{
    public record GetUserByIdCommand(Guid Id):ICommand<GetUserByIdResult>;
    public record GetUserByIdResult(UserDto UserDto);
    public class GetUserByIdHandler(UserManager<ApplicationUser> _userManager,IPublishEndpoint _publishEndPoint) : ICommandHandler<GetUserByIdCommand, GetUserByIdResult>
    {
        public async  Task<GetUserByIdResult> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            if (user == null)
            {
                throw new NotFoundException($"User with Id {request.Id} is not found");
            }
            return new GetUserByIdResult(new UserDto(user.Id,user.UserName,user.Email,user.Gil,user.CreatedOn));
        }
    }
}
