using buildingBlock.Messaging.Events.PostPublishEvents;

namespace postQuery.Infrastractur.Handlers
{
    public interface IEventPubHandler
    {
        Task On(PostCreatedPubEvent @event);
        Task On(MessageUpdatedPubEvent @event);
        Task On(PostLikedPubEvent @event);
        Task On(CommentAddedPubEvent @event);
        Task On(CommentUpdatedPubEvent @event);
        Task On(CommentRemovePubEvent @event);
        Task On(PostRemovedPubEvent @event);
    }
}
