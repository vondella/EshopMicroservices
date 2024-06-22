namespace postcmd.posts.Events
{
    public class PostEventCreated:BaseEvent
    {
        public PostEventCreated():base(nameof(PostEventCreated))
        {
            
        }
        public string Author { get; set; }
        public string Message { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
