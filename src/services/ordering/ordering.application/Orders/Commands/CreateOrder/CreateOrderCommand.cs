
namespace ordering.application.Orders.Commands.CreateOrder;
public record CreateOrderCommand(OrderDto Order):ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid Id);


public class CreateOrderCommandValidator:AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Order Name is required");
        RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("CustomerId is required");
        RuleFor(x => x.Order.OrderItemDtos).NotEmpty().WithMessage("order items should not be empty");

    }

}