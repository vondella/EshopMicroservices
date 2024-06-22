namespace postcmd.Infrastracture.EventHandlers
{
    public class EventSourcingHandler : IEventSourcingHandler<PostAggregates>
    {
        private readonly IEventStore _eventStore;
        public EventSourcingHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public async Task<PostAggregates> GetById(Guid Id)
        {
            var aggregate = new PostAggregates();
            var events = await _eventStore.GetEventsAsync(Id);
            if (events == null || !events.Any()) return aggregate;

            aggregate.ReplayEvents(events);
            aggregate.Version = events.Select(x => x.Version).Max();
            return aggregate;
        }

        public async Task SaveAsync(AggregateRoot aggregateRoot)
        {
            await _eventStore.SaveEventAsync(aggregateRoot.Id, aggregateRoot.GetUnCommitedChanges(), aggregateRoot.Version);
            aggregateRoot.MarkChangesCommited();
        }
    }
}
