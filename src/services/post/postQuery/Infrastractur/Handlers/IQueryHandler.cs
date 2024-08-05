using postQuery.Infrastractur.Queries;

namespace postQuery.Infrastractur.Handlers
{
    public interface IQueryHandler
    {
        Task<List<PostEntity>> HandleAsync(FindAllQuery query);
        Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query);
        Task<List<PostEntity>> HandleAsync(FindPostByAuthorQuery query);
        Task<List<PostEntity>> HandleAsync(FindPostWithCommentQuery query);
        Task<List<PostEntity>> HandleAsync(FindPostWithLikesQuery query);

    }
}
