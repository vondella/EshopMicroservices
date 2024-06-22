using buildingBlock.CQRS.Commands;

namespace postcmd.posts.Commands
{
    public class DeletePostCommand:BaseCommand
    {
        public string Username { get; set; }
    }
}
