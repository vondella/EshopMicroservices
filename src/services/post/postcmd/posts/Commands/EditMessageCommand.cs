using buildingBlock.CQRS.Commands;

namespace postcmd.posts.Commands
{
    public class EditMessageCommand:BaseCommand
    {
        public string Message { get; set; }
    }
}
