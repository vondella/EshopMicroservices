
using buildingBlock.Exceptions;
using Play.Identity.Contract;

namespace Play.Identity.Service.Users.RemoveUser
{
    public record RemoveUserCommand(Guid Id):ICommand<RemoveUserResult>;
    public record RemoveUserResult(bool Deleted);
    public class RemoveUserHandler(UserManager<ApplicationUser> userManager, IPublishEndpoint publishEndpoint) : ICommandHandler<RemoveUserCommand, RemoveUserResult>
    {
        public async  Task<RemoveUserResult> Handle(RemoveUserCommand command, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(command.Id.ToString());
            if (user == null)
                throw new NotFoundException($"the user with Id {command.Id} is not found");

            await userManager.DeleteAsync(user);
            await publishEndpoint.Publish(new UserUpdated(user.Id, user.Email, 0));

            return new RemoveUserResult(true);
        }
    }
}
