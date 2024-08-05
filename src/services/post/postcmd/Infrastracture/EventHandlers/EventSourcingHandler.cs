using MassTransit;
using MassTransit.Transports;

namespace postcmd.Infrastracture.EventHandlers
{
    public class EventSourcingHandler : IEventSourcingHandler<PostAggregates>
    {
        private readonly IEventStore _eventStore;
        private readonly IPublishEndpoint _endPoint;
        public EventSourcingHandler(IEventStore eventStore, IPublishEndpoint endPoint)
        {
            _eventStore = eventStore;
            _endPoint = endPoint;
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

        public async Task RepublishEventAsync()
        {
            var aggregateIds = await _eventStore.GetAggregateIdAsync();
            if (aggregateIds == null && !aggregateIds.Any()) return;

            foreach(var aggregateId in aggregateIds)
            {
                var aggregate = await GetById(aggregateId);
                if (aggregate == null && !aggregateIds.Any()) continue;
                var events = await _eventStore.GetEventsAsync(aggregateId);
                foreach(var @event in events)
                {
                    await PublishEvent.PublishAsync(_endPoint, @event);
                }
            }
        }

        public async Task SaveAsync(AggregateRoot aggregateRoot)
        {
            await _eventStore.SaveEventAsync(aggregateRoot.Id, aggregateRoot.GetUnCommitedChanges(), aggregateRoot.Version);
            aggregateRoot.MarkChangesCommited();
        }
    }
}
