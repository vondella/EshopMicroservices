using postcmd.posts.Commands;

namespace postcmd.posts.CommandHandlers
{
    public class EventCommandHandler : IEventCommandHandler
    {
        private readonly IEventSourcingHandler<PostAggregates> _eventSourcingHandler;
        public EventCommandHandler(IEventSourcingHandler<PostAggregates> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }
        public async Task HandleAsync(NewPostCommand command)
        {
            var aggregate = new PostAggregates(command.Id, command.Author, command.Message);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(DeletePostCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetById(command.Id);
            aggregate.DeletePost(command.Username);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditCommentCommand command)
        {
            var aggregate = await  _eventSourcingHandler.GetById(command.Id);
            aggregate.EditComment(command.CommentId, command.Comment, command.Username);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditMessageCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetById(command.Id);
            aggregate.EditMessage(command.Message);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(LikePostCommand command)
        {
            var aggregate =  await  _eventSourcingHandler.GetById(command.Id);
            aggregate.LikePost();
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RemoveCommentCommand command)
        {
            var aggregate = await  _eventSourcingHandler.GetById(command.Id);
            aggregate.RemoveComment(command.CommentId, command.Username);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(AddCommentCommand command)
        {
            var aggregate = await  _eventSourcingHandler.GetById(command.Id);
            aggregate.AddComment(command.Comment,command.UserName);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }
    }
}
