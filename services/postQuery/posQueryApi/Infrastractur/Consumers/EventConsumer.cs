

using posQueryApi.Converters;
using System.Text.Json;

namespace posQueryApi.Infrastractur.Consumers
{
    public class EventConsumer : IEventConsumer
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly IEventHandler _eventHandler;
        public EventConsumer(IOptions<ConsumerConfig> consumerConfig,IEventHandler eventHandler)
        {
            _consumerConfig = consumerConfig.Value;
            _eventHandler = eventHandler;
        }

        public void Consume(string topic)
        {
            using var consumer =new  ConsumerBuilder<string, string>(_consumerConfig)
                .SetKeyDeserializer(Deserializers.Utf8)
                .SetValueDeserializer(Deserializers.Utf8).Build();
            consumer.Subscribe(topic);
            while(true)
            {
                var consumerResult = consumer.Consume();
                if (consumerResult?.Message == null) continue;
                var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
                var @event = JsonSerializer.Deserialize<BaseEvent>(consumerResult.Message.Value, options);
                var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { @event.GetType() });
                if(handlerMethod == null)
                {
                    throw new ArgumentException(nameof(handlerMethod), "Couldnot find handler method");
                }
                handlerMethod.Invoke(_eventHandler, new object[] { @event });
                consumer.Commit(consumerResult);
            }
        }
    }
}
