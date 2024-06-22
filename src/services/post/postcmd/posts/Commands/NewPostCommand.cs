using buildingBlock.CQRS.Commands;

namespace postcmd.posts.Commands
{
    public class NewPostCommand:BaseCommand
    {
        public string Author { get; set; }
        public string Message { get; set; }
    }
}
