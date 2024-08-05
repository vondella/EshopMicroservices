using buildingBlock.CQRS;
using FluentValidation;

namespace postcmd.posts.PostApiEndPoints.LikePost
{
    public record LikePostCommand(Guid Id):ICommand<LikePostResponse>;
    public record LikePostResponse(Guid Id,string Message);
    public class LikePostCommandValidator:AbstractValidator<LikePostCommand>
    {
        public LikePostCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Id is required");
        }
    }
    public class LikePostHandler(IEventSourcingHandler<PostAggregates> _eventSourcingHandler) : ICommandHandler<LikePostCommand, LikePostResponse>
    {
        public async Task<LikePostResponse> Handle(LikePostCommand command, CancellationToken cancellationToken)
        {
            var aggregate =  await  _eventSourcingHandler.GetById(command.Id);
            aggregate.LikePost();
            await _eventSourcingHandler.SaveAsync(aggregate);
            return new LikePostResponse(command.Id, Message: $"A post with id no {command.Id} has been liked successfully");
        }
    }
}
