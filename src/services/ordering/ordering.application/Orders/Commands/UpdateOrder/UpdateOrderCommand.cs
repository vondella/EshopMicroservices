
namespace ordering.application.Orders.Commands.UpdateOrder;
public record UpdateOrderCommand(OrderDto orderDto):ICommand<UpdateOrderResult>;
public record UpdateOrderResult(bool IsSuccess);
public class UpdateOrderCommandValidator:AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.orderDto.OrderName).NotEmpty().WithMessage("Order Name is required");
        RuleFor(x => x.orderDto.CustomerId).NotNull().WithMessage("CustomerId is required");
        RuleFor(x => x.orderDto.OrderItemDtos).NotEmpty().WithMessage("order items should not be empty");
    }
}