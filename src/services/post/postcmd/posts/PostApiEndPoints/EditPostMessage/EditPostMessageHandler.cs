using buildingBlock.CQRS;
using FluentValidation;

namespace postcmd.posts.PostApiEndPoints.EditPostMessage
{
    public record EditPostMessageCommand(Guid Id,string Message):ICommand<EditPostMessageResponse>;
    public record EditPostMessageResponse(Guid Id,string Message);
    public class EditPostMessageCommandValidator:AbstractValidator<EditPostMessageCommand>
    {
        public EditPostMessageCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Id is required");
            RuleFor(x => x.Message).NotEmpty().WithMessage("Message is required");
        }

    }
    public class EditPostMessageHandler(IEventSourcingHandler<PostAggregates> _eventSourcingHandler) : ICommandHandler<EditPostMessageCommand, EditPostMessageResponse>
    {
        public async Task<EditPostMessageResponse> Handle(EditPostMessageCommand command, CancellationToken cancellationToken)
        {
            var aggregate =  await  _eventSourcingHandler.GetById(command.Id);
            aggregate.EditMessage(command.Message);
            await _eventSourcingHandler.SaveAsync(aggregate);
            return new EditPostMessageResponse(command.Id,Message: "Post message has been edited successfully");
        }
    }
}
