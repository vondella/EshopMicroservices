using buildingBlock.CQRS;
using FluentValidation;
using MongoDB.Driver;

namespace postcmd.posts.PostApiEndPoints.EditPostComment
{
    public record EditCommentCommand(Guid Id,Guid CommentId,string Comment,string Username) :ICommand<EditCommentResponse>;
    public record EditCommentResponse(Guid Id,string Message);
    public class EditCommentCommandValidator:AbstractValidator<EditCommentCommand>
    {
        public EditCommentCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("username is required");
            RuleFor(x => x.Comment).NotEmpty().WithMessage("comment is required");
            RuleFor(x => x.CommentId).NotNull().NotEmpty().WithMessage("commentId is required");
        }
    }
    public class EditCommentHandler(IEventSourcingHandler<PostAggregates> _eventSourcingHandler) : ICommandHandler<EditCommentCommand, EditCommentResponse>
    {
        public async Task<EditCommentResponse> Handle(EditCommentCommand command, CancellationToken cancellationToken)
        {
            var aggregate = await _eventSourcingHandler.GetById(command.Id);
            aggregate.EditComment(command.CommentId, command.Comment, command.Username);
            await _eventSourcingHandler.SaveAsync(aggregate);
             return  new EditCommentResponse(aggregate.Id,Message:"Post message has been edited successfully");
        }
    }
}
