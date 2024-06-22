using buildingBlock.CQRS.Commands;

namespace postcmd.posts.Commands
{
    public class RemoveCommentCommand:BaseCommand
    {
        public Guid CommentId { get; set; }
        public string Username { get; set; }
    }
}
