using buildingBlock.Exceptions;

namespace basketApi.Exceptions
{
    public class BasketNotFoundException:NotFoundException
    {
        public BasketNotFoundException(string userName):base("Basket",userName)
        {
            
        }
    }
}
