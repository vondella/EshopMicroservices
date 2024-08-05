using postQuery.Infrastractur.Queries;
using ZstdSharp.Unsafe;

namespace postQuery.Infrastractur.Handlers
{
    public class QueryHandler : IQueryHandler
    {
        private readonly IPostRepository _repository;

        public QueryHandler(IPostRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PostEntity>> HandleAsync(FindAllQuery query)
        {
            return await _repository.ListAllAsync();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
        {
            var post = await _repository.GetByIdAsync(query.Id);
            return new List<PostEntity> { post };
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostByAuthorQuery query)
        {
            return await _repository.ListByAuthorAsync(query.Author);
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostWithCommentQuery query)
        {
            return await _repository.ListWithCommentsAsync();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostWithLikesQuery query)
        {
            return await _repository.ListWithLikesAsync(query.NumberOfLikes);
        }
    }
}
