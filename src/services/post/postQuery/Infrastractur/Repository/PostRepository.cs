
namespace posQuery.Infrastracture.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DatabaseContextFactory _databaseContextFactory;

        public PostRepository(DatabaseContextFactory databaseContextFactory)
        {
            _databaseContextFactory = databaseContextFactory;
        }

        public async Task CreateAsync(PostEntity post)
        {
            using var context = _databaseContextFactory.CreateDbContext();
            context.Posts.Add(post);
            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid PostId)
        {
            using var context = _databaseContextFactory.CreateDbContext();
            var post = await GetByIdAsync(PostId);
            if (post == null) return;
            context.Remove(post);
            _ = await context.SaveChangesAsync();
        }

        public async Task<PostEntity> GetByIdAsync(Guid PostId)
        {
            using var context = _databaseContextFactory.CreateDbContext();
            return await context.Posts.Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.PostId == PostId);

        }

        public async Task<List<PostEntity>> ListAllAsync()
        {
            using var context = _databaseContextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking().Include(x => x.Comments).ToListAsync();                
        }

        public async Task<List<PostEntity>> ListByAuthorAsync(string Author)
        {
            using var context = _databaseContextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                .Include(x => x.Comments).AsNoTracking()
                .Where(x => x.Author.Contains(Author)).ToListAsync();
        }

        public async Task<List<PostEntity>> ListWithCommentsAsync()
        {
            using var context = _databaseContextFactory.CreateDbContext();
            return  await  context.Posts.AsNoTracking()
                .Include(x => x.Comments).AsNoTracking()
                .Where(x => x.Comments != null && x.Comments.Any()).ToListAsync();
        }

        public async Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes)
        {
            using var context = _databaseContextFactory.CreateDbContext();
            return await  context.Posts.AsNoTracking()
                .Include(x => x.Comments).AsNoTracking()
                .Where(x => x.Likes >= numberOfLikes).ToListAsync();
        }

        public async Task UpdateAsync(PostEntity post)
        {
            using var context = _databaseContextFactory.CreateDbContext();
            context.Update(post);
            _ = await context.SaveChangesAsync();
        }
    }
}
