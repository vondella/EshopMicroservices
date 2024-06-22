

namespace posQueryApi.Domains.Repositories
{
    public interface ICommentRepository
    {
        Task CreateAsync(CommentEntity comment);
        Task UpdateAsync(CommentEntity comment);
        Task<CommentEntity> GetByIdAsync(Guid CommentId);
        Task DeleteAsync(Guid CommentId);
    }
}
