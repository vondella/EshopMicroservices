

namespace ordering.domain.Exceptions
{
    public  class DomainException:Exception
    {
        public DomainException(string message):base($"Domain Exception {message} throws from domain layer")
        {
                      
        }
    }
}
