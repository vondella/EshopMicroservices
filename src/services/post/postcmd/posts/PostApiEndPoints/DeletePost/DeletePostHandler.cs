using buildingBlock.CQRS;
using FluentValidation;
using postcmd.posts.PostApiEndPoints.AddPost;

namespace postcmd.posts.PostApiEndPoints.DeletePost
{
    public record DeletePostCommand(Guid Id,string Username) : ICommand<DeletePostResponse>;
    public record DeletePostResponse();
    public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
    {
        public DeletePostCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username  is required");
        }
    }
    public class DeletePostHandler(IEventSourcingHandler<PostAggregates> _eventSourcingHandler) : ICommandHandler<DeletePostCommand, DeletePostResponse>
    {
        public async Task<DeletePostResponse> Handle(DeletePostCommand command, CancellationToken cancellationToken)
        {
            var aggregate = await _eventSourcingHandler.GetById(command.Id);
            aggregate.DeletePost(command.Username);
             await  _eventSourcingHandler.SaveAsync(aggregate);

            throw new NotImplementedException();
        }
    }
}
