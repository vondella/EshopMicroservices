using MassTransit;
using Microsoft.FeatureManagement;
using ordering.application.Extensions;

namespace ordering.application.Orders.EventHandlers.Domain
{
    public class OrderCreatedEventHandler(IPublishEndpoint publishEndPoint,IFeatureManager featureManager,ILogger<OrderCreatedEvent> logger) : INotificationHandler<OrderCreatedEvent>
    {
        public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event Handled {DomainEvent}", notification.GetType().Name);
            if(await featureManager.IsEnabledAsync("OrderFullfilment"))
            {
                var orderCreatedEvent = notification.order.ToOrderDto();
                await publishEndPoint.Publish(orderCreatedEvent, cancellationToken);
            }
           
        }
    }
}