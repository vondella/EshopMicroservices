using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ordering.application.Orders.Queries.GetOrdersByCustomer;
public record GetOrderByCustomerQuery(Guid CustomerId):IQuery<GetOrdersByCustomerResult>;
public record GetOrdersByCustomerResult(IEnumerable<OrderDto> orders);
