

namespace postcmd.posts.PostApiEndPoints.AddPost
{
    public record AddPostCommand(string Author, string Message):ICommand<AddPostResponse>;
    public record AddPostResponse(Guid Id,string Message);
    public class AddPostCommandValidator : AbstractValidator<AddPostCommand>
    {
        public AddPostCommandValidator()
        {
            RuleFor(x => x.Author).NotEmpty().WithMessage("Author is required");
            RuleFor(x => x.Message).NotEmpty().WithMessage("Message is required");        
        }
    }
    public class AddPostHandler(IEventSourcingHandler<PostAggregates> _eventSourcingHandler) : ICommandHandler<AddPostCommand, AddPostResponse>
    {
        public async Task<AddPostResponse> Handle(AddPostCommand command, CancellationToken cancellationToken)
        {
            var aggregate = new PostAggregates(id:Guid.NewGuid(), command.Author, command.Message);
             await  _eventSourcingHandler.SaveAsync(aggregate);
            return new AddPostResponse(aggregate.Id, Message: "new post has ben added successfully");
        }
    }
}
