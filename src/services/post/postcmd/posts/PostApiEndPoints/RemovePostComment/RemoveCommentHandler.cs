using buildingBlock.CQRS;
using FluentValidation;

namespace postcmd.posts.PostApiEndPoints.RemovePostComment
{
    public record RemoveCommentCommand(Guid Id,Guid CommentId,string Username):ICommand<RemoveCommentResponse>;
    public record RemoveCommentResponse(Guid CommentId, string Message);
    public class RemoveCommentCommandValidator:AbstractValidator<RemoveCommentCommand>
    {
        public RemoveCommentCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id is required");
            RuleFor(x => x.CommentId).NotNull().NotEmpty().WithMessage("CommentId is required");
            RuleFor(x => x.Username).NotEmpty().WithMessage("username is required");
        }
    }
    public class RemoveCommentHandler(IEventSourcingHandler<PostAggregates> _eventSourcingHandler) : ICommandHandler<RemoveCommentCommand, RemoveCommentResponse>
    {
        public async Task<RemoveCommentResponse> Handle(RemoveCommentCommand command, CancellationToken cancellationToken)
        {
            var aggregate = await _eventSourcingHandler.GetById(command.Id);
            aggregate.RemoveComment(command.CommentId, command.Username);
            await  _eventSourcingHandler.SaveAsync(aggregate);
            return new RemoveCommentResponse(command.CommentId,Message: $"Comment with Id:{command.CommentId} has been removed successfully");
        }
    }
}
