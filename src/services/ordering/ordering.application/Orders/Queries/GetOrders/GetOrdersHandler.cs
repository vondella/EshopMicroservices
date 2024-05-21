﻿using buildingBlock.Pagination;
using ordering.application.Extensions;
using ordering.application.Orders.Queries.GetOrdersByName;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ordering.application.Orders.Queries.GetOrders
{
    public class GetOrdersHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
    {
        public async Task<GetOrdersResult> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var pageIndex = request.paginationRequest.PageIndex;
            var pageSize = request.paginationRequest.PageSize;

            var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);

            var orders = await dbContext.Orders.Include(o => o.OrderItems)
                .OrderBy(o => o.OrderName.Value)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new GetOrdersResult(new PaginatedResult<OrderDto>(pageIndex,pageSize,totalCount,orders.ToOrderDtoList()));
        }
    }
}
