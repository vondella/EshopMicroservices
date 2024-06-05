using Microsoft.AspNetCore.Identity;

namespace authApi.Model
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
