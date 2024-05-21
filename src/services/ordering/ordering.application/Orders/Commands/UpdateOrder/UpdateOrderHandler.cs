

using ordering.application.Exceptions;
using ordering.domain.ValueObjects;

namespace ordering.application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(command.orderDto.Id);
            var order = await dbContext.Orders.FindAsync([orderId], cancellationToken:cancellationToken);

            if(order is null)
            {
                throw new OrderNotFoundException(command.orderDto.Id);

            }
            UpdateOrderWithNewValue(order, command.orderDto);
            dbContext.Orders.Update(order);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateOrderResult(true);
        }
        public void UpdateOrderWithNewValue(Order order,OrderDto orderDto)
        {
            var updateShippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine,
                orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);

            var updateBillingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine,
                orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);

            var updatePayment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.CVV, orderDto.Payment.PaymentMethod);

            order.Update(orderName: OrderName.Of(orderDto.OrderName),
                shippingAddress: updateShippingAddress,
                billingAddress: updateBillingAddress,
                payment: updatePayment,
                status: orderDto.OrderStatus);
        }
    }
}
