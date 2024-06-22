﻿using buildingBlock.CQRS;
using FluentValidation;

namespace postcmd.posts.PostApiEndPoints.AddPostComment
{
    public record AddPostCommentCommand(Guid Id,string Comment,string UserName):ICommand<AddPostCommentResponse>;
    public record AddPostCommentResponse();
    public class AddPostCommentCommandValidator:AbstractValidator<AddPostCommentCommand>
    {
        public AddPostCommentCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id is required");
            RuleFor(x => x.Comment).NotEmpty().WithMessage("Comment is required");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("username is requored");
        }
    }
    public class AddPostCommentHandler(IEventSourcingHandler<PostAggregates> _eventSourcingHandler) : ICommandHandler<AddPostCommentCommand, AddPostCommentResponse>
    {
        public async Task<AddPostCommentResponse> Handle(AddPostCommentCommand command, CancellationToken cancellationToken)
        {
            var aggregate =  await  _eventSourcingHandler.GetById(command.Id);
            aggregate.AddComment(command.Comment, command.UserName);
            await _eventSourcingHandler.SaveAsync(aggregate);

            throw new NotImplementedException();
        }
    }
}