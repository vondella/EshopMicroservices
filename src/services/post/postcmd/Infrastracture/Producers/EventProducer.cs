

namespace postcmd.Infrastracture.Producers
{
    public class EventProducer : IEventProducer
    {
        private readonly ProducerConfig _config;
        public EventProducer(IOptions<ProducerConfig> config)
        {
            _config = config.Value;
        }
        public async Task ProduceAsync<T>(string Topic, T @event) where T : BaseEvent
        {
            using var producer = new ProducerBuilder<string, string>(_config)
                .SetKeySerializer(Serializers.Utf8)
                .SetValueSerializer(Serializers.Utf8)
                .Build();

            var eventMessage = new Message<string, string>
            {
                Key=Guid.NewGuid().ToString(),
                Value=JsonSerializer.Serialize(@event,@event.GetType())
            };
            var deliveryResult = await producer.ProduceAsync(Topic, eventMessage);

            if(deliveryResult.Status== PersistenceStatus.NotPersisted)
            {
                throw new Exception($"could not produce {@event.GetType().Name}  message to topic -{Topic} due to the following reason: {deliveryResult.Message} ");
            }
        }
    }
}
