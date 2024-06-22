
namespace postcmd.Domains
{
    public abstract class AggregateRoot
    {
        protected Guid id;
        private readonly List<BaseEvent> changes = new List<BaseEvent>();
        public Guid Id { get { return id; } }
        public int Version { get; set; } = -1;
        public IEnumerable<BaseEvent> GetUnCommitedChanges()
        {
            return changes;
        }
        private void ApplyChanges(BaseEvent @event, bool IsNew)
        {
            var method = this.GetType().GetMethod("Apply",new Type[] {@event.GetType()});
            if(method == null)
            {
                throw new ArgumentNullException(nameof(method),
                    $"The apply method was not found in the aggregate for {@event.GetType().Name}");
            }
            method.Invoke(this, new object[] { @event });
            if(IsNew)
            {
                changes.Add(@event);
            }
        }
        public void MarkChangesCommited()
        {
            changes.Clear();
        }
        protected void RaiseEvent(BaseEvent @event)
        {
            ApplyChanges(@event, true);
        }
        public void ReplayEvents(IEnumerable<BaseEvent> events)
        {
            foreach(var @event in events)
            {
                ApplyChanges(@event, false);
            }
        }
         

    }
}
