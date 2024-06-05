using buildingBlock.Exceptions;

namespace authApi.Exceptions
{
    public class UserNotFound: NotFoundException
    {
        public UserNotFound(string userName) : base("auth", userName)
        {

        }
    }
}
