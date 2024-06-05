namespace authApi.Exceptions
{
    public class InvalidUserException:BadHttpRequestException
    {
        public InvalidUserException(string details):base(details)
        {
            
        }
    }
}
