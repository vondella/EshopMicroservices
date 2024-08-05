using buildingBlock.CQRS;
using postQuery.Infrastractur.Helpers;

namespace postQuery.PostQuery.GetAllPost
{
    public record GetAllPostQuery(int? PageNumber, int? PageSize) :IQuery<GetAllPostResult>;
    public record GetAllPostResult(IEnumerable<PostEntity> Posts);
    public class GetAllPostHandler(IPostRepository postRepository) : IQueryHandler<GetAllPostQuery, GetAllPostResult>
    {
        public async  Task<GetAllPostResult> Handle(GetAllPostQuery query, CancellationToken cancellationToken)
        {
            var posts =  await postRepository.ListAllAsync();
            var pagedPosts=posts.ToPagedList(query.PageNumber ?? 1, query.PageSize ?? 10);
             return new GetAllPostResult(pagedPosts);
        }
    }
}
