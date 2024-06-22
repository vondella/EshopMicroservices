namespace postcmd.posts.Events
{
    public abstract class BaseEvent
    {
        public BaseEvent(string type)
        {
            Type = type;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Type { get; set; }
    }
}
