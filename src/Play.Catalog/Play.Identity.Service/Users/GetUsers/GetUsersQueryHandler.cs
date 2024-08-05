

namespace Play.Identity.Service.Users.GetUsers
{
    public record GetUsersQuery(PaginationRequest paginationRequest) : IQuery<GetUsersResult>;
    public record GetUsersResult(PaginatedResult<UserDto> Users);
    public class GetUsersHandler(UserManager<ApplicationUser> userManager) : IQueryHandler<GetUsersQuery, GetUsersResult>
    {
        public async  Task<GetUsersResult> Handle(GetUsersQuery query, CancellationToken cancellationToken)
        {
            var pageIndex = query.paginationRequest.PageIndex;
            var pageSize = query.paginationRequest.PageSize;
            var totalCount =   userManager.Users.ToList();
            var list = userManager.Users.Skip(pageSize * pageIndex).Take(pageSize).ToList();

            return new GetUsersResult(new PaginatedResult<UserDto>(pageIndex, pageSize, totalCount.LongCount(), list.AsDto()));
        }
    }
}
