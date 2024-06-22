using buildingBlock.CQRS;
using FluentValidation;

namespace postcmd.posts.PostApiEndPoints.LikePost
{
    public record LikePostCommand(Guid Id):ICommand<LikePostResponse>;
    public record LikePostResponse();
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
            throw new NotImplementedException();
        }
    }
}
