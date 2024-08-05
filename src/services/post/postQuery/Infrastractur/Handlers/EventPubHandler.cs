using buildingBlock.Messaging.Events.PostPublishEvents;

namespace postQuery.Infrastractur.Handlers
{
    public class EventPubHandler : IEventPubHandler
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        public EventPubHandler(ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }

        public async  Task On(PostCreatedPubEvent @event)
        {
            var post = new PostEntity
            {
                PostId = @event.Id,
                Author = @event.Author,
                DatePosted = @event.DatePosted,
                Message = @event.Message
            };
            await _postRepository.CreateAsync(post);
        }

        public async Task On(MessageUpdatedPubEvent @event)
        {
            var post =  await  _postRepository.GetByIdAsync(@event.Id);
            if (post == null) return;

            post.Message = @event.Message;
            await _postRepository.UpdateAsync(post);
        }

        public async  Task On(PostLikedPubEvent @event)
        {
            var post = await _postRepository.GetByIdAsync(@event.Id);
            if (post == null) return;

            post.Likes++;
            await _postRepository.UpdateAsync(post);
        }

        public async Task On(CommentAddedPubEvent @event)
        {
            var comment = new CommentEntity
            {
                Comment = @event.Comment,
                CommentId = @event.CommentId,
                PostId = @event.Id,
                Edited = false,
                Username = @event.UserName,
                CommentDate = @event.CommentDate
            };
            await _commentRepository.CreateAsync(comment);
        }

        public async  Task On(CommentUpdatedPubEvent @event)
        {
            var comment = await _commentRepository.GetByIdAsync(@event.CommentId);
            if (comment == null) return;
            comment.Comment = @event.Comment;
            comment.Edited = true;
            comment.CommentDate = @event.EditDate;
            await _commentRepository.UpdateAsync(comment);
        }

        public async Task On(CommentRemovePubEvent @event)
        {
            await _commentRepository.DeleteAsync(@event.CommentId);
        }

        public async  Task On(PostRemovedPubEvent @event)
        {
            await _postRepository.DeleteAsync(@event.Id);
        }
    }
}
