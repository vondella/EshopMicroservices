using buildingBlock.Messaging.Events.PostPublishEvents;
using MassTransit;

namespace postcmd.Infrastracture
{
    public static class PublishEvent
    {

        public  static async Task PublishAsync(IPublishEndpoint publishEndpoint,BaseEvent baseEvent)
        {
       
            switch(baseEvent.GetType().Name)
            {
                case nameof(PostEventCreated):
                    await publishEndpoint.Publish(baseEvent.Adapt<PostCreatedPubEvent>());
                    break;
                case nameof(MessageUpdatedEvent):
                    await publishEndpoint.Publish(baseEvent.Adapt<MessageUpdatedPubEvent>());
                    break;
                case nameof(PostLikedEvent):
                    await publishEndpoint.Publish(baseEvent.Adapt<PostLikedPubEvent>());
                    break;
                case nameof(CommentAddedEvent):
                    await publishEndpoint.Publish(baseEvent.Adapt<CommentAddedPubEvent>());
                    break;
                case nameof(CommentUpdatedEvent):
                    await publishEndpoint.Publish(baseEvent.Adapt<CommentUpdatedPubEvent>());
                    break;
                case nameof(CommentRemovedEvent):
                    await publishEndpoint.Publish(baseEvent.Adapt<CommentRemovePubEvent>());
                    break;
                default:
                    await publishEndpoint.Publish(baseEvent.Adapt<PostRemovedPubEvent>());
                    break;

            }
        }
    }
}
