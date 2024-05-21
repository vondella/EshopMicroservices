

using buildingBlock.Pagination;

namespace ordering.application.Orders.Queries.GetOrders;
public record GetOrdersQuery(PaginationRequest paginationRequest):IQuery<GetOrdersResult>;
public record GetOrdersResult(PaginatedResult<OrderDto> Orders);
