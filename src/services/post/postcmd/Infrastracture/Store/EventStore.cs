﻿

using buildingBlock.Messaging.Events.PostPublishEvents;
using buildingBlock.Messaging.Producers;
using MassTransit;
using Microsoft.Extensions.Options;
using postcmd.Domains.Aggregates;

namespace postcmd.Infrastracture.Store
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        //private readonly IEventProducer _eventProducer;
        private readonly IPublishEndpoint _publishEndpoint;

        public EventStore(IEventStoreRepository eventStoreRepository, IPublishEndpoint publishEndpoint)
        {
            _eventStoreRepository = eventStoreRepository;
            //_eventProducer = eventProducer;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
        {
            var eventStream = await _eventStoreRepository.FindByAggregate(aggregateId);
            if (eventStream == null || !eventStream.Any())
                throw new AggregateNotFoundException("Incorrect AggregateId Provided");

            return eventStream.OrderBy(x => x.Version).Select(x => x.EvenData).ToList();
        }

        public async Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var eventStream = await _eventStoreRepository.FindByAggregate(aggregateId);
            if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
                throw new ConcurrencyExecption("");

            var version = expectedVersion;
            foreach(var @event in events)
            {
                version ++; 
                @event.Version = version;
                var eventType = @event.GetType().Name;
                var eventModel = new EventModel
                {
                     Timestamp=DateTime.Now,
                     AggregateIdentifier=aggregateId,
                     AggregateType=nameof(PostAggregates),
                     Version=version,
                     EventType=eventType,
                     EvenData=@event
                };
                await _eventStoreRepository.SaveAsync(eventModel);
                //await PublishEvent.PublishAsync(_publishEndpoint, @event);
                await _publishEndpoint.Publish(@event);
                
            }
        }
    }
}