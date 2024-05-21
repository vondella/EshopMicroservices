

using ordering.application.Dtos;
using ordering.domain.Enums;

public record  OrderDto( Guid Id,
    Guid CustomerId,
    string OrderName,
    AddressDto ShippingAddress,
    AddressDto BillingAddress,
    PaymentDto Payment,
    OrderStatus OrderStatus,
    List<OrderItemDto> OrderItemDtos
    );