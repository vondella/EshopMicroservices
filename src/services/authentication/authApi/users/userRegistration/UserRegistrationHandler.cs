
using authApi.Exceptions;
using FluentValidation;

namespace authApi.users.userRegistration
{
    public record UserRegistrationCommand(string Email,string Name,string PhoneNumber,string Password):ICommand<UserRegistrationResult>;
    public record UserRegistrationResult(bool IsSuccess, string Id, string Name, string Email, string PhoneNumber, string Message);
    public class UserRegistrationCommandValidator : AbstractValidator<UserRegistrationCommand>
    {
        public UserRegistrationCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("email is required");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("phone number is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        }
    }
    public class UserRegistrationHandler(AppDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : ICommandHandler<UserRegistrationCommand, UserRegistrationResult>
    {
        public async Task<UserRegistrationResult> Handle(UserRegistrationCommand command, CancellationToken cancellationToken)
        {
            ApplicationUser applicationUser = new()
            {
                Email = command.Email,
                UserName=command.Email,
                Name = command.Name,
                PhoneNumber = command.PhoneNumber,
                NormalizedEmail = command.Email.ToUpper()
            };

            try
            {
                var results = await userManager.CreateAsync(applicationUser, command.Password);
                if(results.Succeeded)
                {
                    var returnedUser = await dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == command.Email);

                    return new UserRegistrationResult(IsSuccess: true, Id: returnedUser.Id, Name: returnedUser.Name, Email: returnedUser.Email, PhoneNumber: returnedUser.PhoneNumber, Message: "");
                      
                }
                throw new UserNotFound("user not registered");
            }
            catch(Exception ex)
            {
                throw new UserNotFound("user  thow exceptions");

            }

        }
    }
}
