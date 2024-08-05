using Play.Identity.Service.Dtos;

namespace Play.Identity.Service.Infrastracture.Extension
{
    public static class Extensions
    {

        public static IEnumerable<UserDto> AsDto(this IEnumerable<ApplicationUser> user)
        {
            return user.Select(userdto => new UserDto(userdto.Id, userdto.UserName, userdto.Email, userdto.Gil, userdto.CreatedOn));
        }
        
    }
}
