using postcmd.posts.Events;

namespace postcmd.Domains.Aggregates
{
    public class PostAggregates:AggregateRoot
    {
        private bool _active;
        private string Author;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new Dictionary<Guid, Tuple<string, string>>();
        public bool Active
        {
            get => _active; set => _active = value;
        }
        public PostAggregates()
        {
            
        }
        public PostAggregates(Guid id,string author,string message)
        {
            RaiseEvent(new PostEventCreated
            {
                Id=id,
                Author=author,
                Message=message,
                 DatePosted=DateTime.Now
            });
        }

        public void Apply(PostEventCreated @event)
        {
            id = @event.Id;
            _active = true;
            Author = @event.Author;
        }
        public void EditMessage(string message)
        {
            if(!_active)
            {
                throw new InvalidOperationException("you can not edit the message of inactive post");
            }
            if(string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidOperationException($"The value of {nameof(message)} cannot be null or empty");
            }
            RaiseEvent(new MessageUpdatedEvent
            {
                Id=Id,
                Message=message
            });
        }
        public void Apply(MessageUpdatedEvent @event)
        {
            id = @event.Id;
        }
        public void LikePost()
        {
            if(!_active)
            {
                throw new InvalidOperationException("you can not like an inactive post");
            }
            RaiseEvent(new PostLikedEvent
            {
                Id=id

            });
        }
        public void Apply(PostLikedEvent @event)
        {
            id = @event.Id;
        }
        public void AddComment(string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("you can not add a comment for inactive post");
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"the value of {nameof(comment)} can not be null or empty");
            }

            RaiseEvent(new CommentAddedEvent
            {
                Id = id,
                Comment = comment,
                CommentId = Guid.NewGuid(),
                UserName = username,
                CommentDate = DateTime.Now
            });
        }
        public void Apply(CommentAddedEvent @event)
        {
            id = @event.Id;
            _comments.Add(@event.CommentId,new Tuple<string,string>(@event.Comment,@event.UserName));
        }

        public void EditComment(Guid commentId,string comment,string username)
        {
            if(!_active)
            {
                throw new InvalidOperationException("you can not edit a comment of inactive post");
            }
            if (!_comments[commentId].Item2.Equals(username,StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("you can not edit a comment which is not yours");
            }
            RaiseEvent(new CommentUpdatedEvent { Id=id,CommentId=commentId,Comment=comment,UserName=username,EditDate=DateTime.Now });
        }
        public void Apply(CommentUpdatedEvent @event)
        {
            id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment,@event.UserName);
        }
        public void RemoveComment(Guid commentId,string username)
        {
            if(!_active)
            {
                throw new InvalidOperationException("you cannot remove comment of inactive post");
            }
            if (!_comments[commentId].Item2.Equals(username,StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("you cannot remove comment which is not yours");
            }
            RaiseEvent(new CommentRemovedEvent { Id=id, CommentId=commentId  });
        }
        public void Apply(CommentRemovedEvent @event)
        {
            id = @event.Id;
            _comments.Remove(@event.CommentId);
        }
        public void DeletePost(string username)
        {
            if(!_active)
            {
                throw new InvalidOperationException("you cannot delete post which is not yours");
           }
            if(! Author.Equals(username,StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("you can not delete post which is not yours");
            }
            RaiseEvent(new PostRemovedEvent { Id = id });
        }
        public void Apply(PostRemovedEvent @event)
        {
            id = @event.Id;
        }
    }
}
