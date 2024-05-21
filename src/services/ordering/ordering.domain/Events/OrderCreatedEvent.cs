
namespace ordering.domain.Events
{
    public record  OrderCreatedEvent(Order order):IDomainEvent;
   
}
