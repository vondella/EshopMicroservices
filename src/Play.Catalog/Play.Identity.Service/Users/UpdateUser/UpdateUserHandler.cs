
using buildingBlock.Exceptions;
using Play.Identity.Contract;
using Play.Identity.Service.Dtos;

namespace Play.Identity.Service.Users.UpdateUser
{
    public record UpdateUserCommand(Guid Id,string Email,decimal Gil):ICommand<UpdateUserResult>;
    public record UpdateUserResult(bool Succeded);
    public class UpdateUserHandler(UserManager<ApplicationUser> userManager, IPublishEndpoint publishEndpoint) : ICommandHandler<UpdateUserCommand, UpdateUserResult>
    {
        public  async   Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(command.Id.ToString());
            if(user == null)
            {
                throw new NotFoundException($"user with Id {command.Id} is not found");
            }
            user.Email = command.Email;
            user.UserName = command.Email;
            user.Gil = command.Gil;
            await userManager.UpdateAsync(user);
            await publishEndpoint.Publish(new UserUpdated(user.Id, user.Email, user.Gil));
            return new UpdateUserResult(true);
        }
    }
}
