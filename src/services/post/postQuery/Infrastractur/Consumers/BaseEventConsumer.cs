using buildingBlock.Messaging.Events.PostPublishEvents;
using MassTransit;
using postQuery.Infrastractur.Handlers;

namespace postQuery.Infrastractur.Consumers
{
    public class BaseEventConsumer : IConsumer<PostCreatedPubEvent>
    {
        private readonly IEventPubHandler _eventHandler;

        public BaseEventConsumer(IEventPubHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }


        public async  Task Consume(ConsumeContext<PostCreatedPubEvent> context)
        {
            var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { context.Message.GetType() });
            if (handlerMethod == null)
            {
                throw new ArgumentException(nameof(handlerMethod), "Couldnot find handler method");
            }
             handlerMethod.Invoke(_eventHandler, new object[] { context.Message });
        }
    }
    public class EditPostMessageConsumer : IConsumer<MessageUpdatedPubEvent>
    {
        private readonly IEventPubHandler _eventHandler;

        public EditPostMessageConsumer(IEventPubHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public async  Task Consume(ConsumeContext<MessageUpdatedPubEvent> context)
        {
            var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { context.Message.GetType() });
            if (handlerMethod == null)
            {
                throw new ArgumentException(nameof(handlerMethod), "Couldnot find handler method");
            }
            handlerMethod.Invoke(_eventHandler, new object[] { context.Message });
        }
    }

    public class LikePostConsumer : IConsumer<PostLikedPubEvent>
    {
        private readonly IEventPubHandler _eventHandler;

        public LikePostConsumer(IEventPubHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public async  Task Consume(ConsumeContext<PostLikedPubEvent> context)
        {
            var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { context.Message.GetType() });
            if (handlerMethod == null)
            {
                throw new ArgumentException(nameof(handlerMethod), "Couldnot find handler method");
            }
            handlerMethod.Invoke(_eventHandler, new object[] { context.Message });
        }
    }

    public class AddCommentConsumer : IConsumer<CommentAddedPubEvent>
    {
        private readonly IEventPubHandler _eventHandler;

        public AddCommentConsumer(IEventPubHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }
        public async  Task Consume(ConsumeContext<CommentAddedPubEvent> context)
        {
            var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { context.Message.GetType() });
            if (handlerMethod == null)
            {
                throw new ArgumentException(nameof(handlerMethod), "Couldnot find handler method");
            }
            handlerMethod.Invoke(_eventHandler, new object[] { context.Message });
        }
    }
    public class EditCommentConsumer : IConsumer<CommentUpdatedPubEvent>
    {
        private readonly IEventPubHandler _eventHandler;

        public EditCommentConsumer(IEventPubHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public async  Task Consume(ConsumeContext<CommentUpdatedPubEvent> context)
        {
            var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { context.Message.GetType() });
            if (handlerMethod == null)
            {
                throw new ArgumentException(nameof(handlerMethod), "Couldnot find handler method");
            }
            handlerMethod.Invoke(_eventHandler, new object[] { context.Message });
        }
    }
    public class RemoveCommentConsumer : IConsumer<CommentRemovePubEvent>
    {
        private readonly IEventPubHandler _eventHandler;

        public RemoveCommentConsumer(IEventPubHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public async  Task Consume(ConsumeContext<CommentRemovePubEvent> context)
        {
            var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { context.Message.GetType() });
            if (handlerMethod == null)
            {
                throw new ArgumentException(nameof(handlerMethod), "Couldnot find handler method");
            }
            handlerMethod.Invoke(_eventHandler, new object[] { context.Message });
        }
    }

    public class DeletePostConsumer : IConsumer<PostRemovedPubEvent>
    {
        private readonly IEventPubHandler _eventHandler;

        public DeletePostConsumer(IEventPubHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public async Task Consume(ConsumeContext<PostRemovedPubEvent> context)
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
