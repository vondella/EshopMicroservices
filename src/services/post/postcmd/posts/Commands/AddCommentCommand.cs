using buildingBlock.CQRS.Commands;

namespace postcmd.posts.Commands
{
    public class AddCommentCommand:BaseCommand
    {
        public string Comment { get; set; }
        public string UserName { get; set; }

    }
}
