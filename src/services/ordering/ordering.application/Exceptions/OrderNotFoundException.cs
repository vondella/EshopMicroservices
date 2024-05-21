
using buildingBlock.Exceptions;

namespace ordering.application.Exceptions
{
    public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(Guid id) : base("Order",id)
        {
        }
    }
}
