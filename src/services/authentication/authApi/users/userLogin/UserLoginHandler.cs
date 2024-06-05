

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace authApi.users.userLogin
{
    public record LoginUserCommand(string Username, string Password):ICommand<LoginUserResult>;
    public record LoginUserResult(bool IsSuccess,string Message,string Id, string Name,string Email,string PhoneNumber,string Token);
    public class UserLoginHandler(IOptions<JwtOptions> jwtOptions,AppDbContext dbContext,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager) : ICommandHandler<LoginUserCommand, LoginUserResult>
    {
        public async  Task<LoginUserResult> Handle(LoginUserCommand  command, CancellationToken cancellationToken)
        {
            var user = await dbContext.ApplicationUsers.FirstOrDefaultAsync(u=>u.UserName.ToUpper() == command.Username.ToUpper());
            bool isValid = await userManager.CheckPasswordAsync(user, command.Password);
            if (user == null || isValid == false)
            {
                throw new InvalidUserException("invalid user login");
            }
            var token = GenerateToken(user, jwtOptions);
            return new LoginUserResult(IsSuccess:true,Message:"User logged in successfully",
                Id:user.Id,Name:user.Name,Email:user.Email,PhoneNumber:user.PhoneNumber,Token:token);

        }
        private string GenerateToken(ApplicationUser user,IOptions<JwtOptions> jwtOptions)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtOptions.Value.Secret);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name,user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Name,user.UserName)
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience=jwtOptions.Value.Audience,
                Issuer=jwtOptions.Value.Issuer,
                Subject=new ClaimsIdentity(claims),
                Expires=DateTime.UtcNow.AddDays(1),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
