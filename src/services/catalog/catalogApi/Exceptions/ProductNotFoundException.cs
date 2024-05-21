using buildingBlock.Exceptions;

namespace catalogApi.Exceptions
{
    public class ProductNotFoundException:NotFoundException
    {
        public ProductNotFoundException(Guid Id ):base("product ",Id)
        {
            
        }
    }
}
