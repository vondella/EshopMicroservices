using MassTransit;

namespace postQuery.Infrastractur.Consumers
{
    public class BaseEventConsumer : IConsumer<BaseEvent>
    {
        private readonly IEventHandler _eventHandler;

        public BaseEventConsumer(IEventHandler eventHandler)
        {
            this._eventHandler = eventHandler;
        }

       
        public async  Task Consume(ConsumeContext<BaseEvent> context)
        {
            var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { context.Message.GetType() });
            if (handlerMethod == null)
            {
                throw new ArgumentException(nameof(handlerMethod), "Couldnot find handler method");
            }
             handlerMethod.Invoke(_eventHandler, new object[] { context.Message });
        }
    }
}
