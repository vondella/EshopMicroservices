
using Microsoft.Extensions.Options;

namespace Play.Identity.Service.Infrastracture.HostedService
{
    public class IdentitySeedHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public IdentitySeedHostedService(IServiceScopeFactory serviceScopeFactory, IOptions<IdentitySettings> identitySettings)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _identitySettings = identitySettings.Value;
        }

        private readonly IdentitySettings _identitySettings;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await CreateRoleIfNotExistAsync(Roles.Player, roleManager);
            await CreateRoleIfNotExistAsync(Roles.Admin, roleManager);

            var adminUser =await  userManager.FindByEmailAsync(_identitySettings.AdminUserEmail);

            if(adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = _identitySettings.AdminUserEmail,
                    Email=_identitySettings.AdminUserEmail
                };
                await userManager.CreateAsync(adminUser, _identitySettings.AdminUserPassword);
                await userManager.AddToRoleAsync(adminUser, Roles.Admin);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
      
        private static async Task  CreateRoleIfNotExistAsync(string role,RoleManager<ApplicationRole> roleManager)
        {
            var roleExists = await roleManager.RoleExistsAsync(role);
            if(!roleExists)
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = role });
            }
        }
    }
}
