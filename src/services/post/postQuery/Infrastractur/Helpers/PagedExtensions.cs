namespace postQuery.Infrastractur.Helpers
{
    public static class PagedExtensions
    {
       public static IEnumerable<PostEntity> ToPagedList(this IEnumerable<PostEntity> posts,int PageNumber, int PageSize)
        {
            return posts.Skip(PageNumber - 1).Take(PageSize);
        }
    }
}
