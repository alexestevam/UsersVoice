using System;
using MediatR;

namespace UsersVoice.Services.API.CQRS.Events
{
    public class IdeaCommentCreated : IAsyncNotification
    {
        public IdeaCommentCreated(Guid commentId)
        {
            if (commentId == Guid.Empty) throw new ArgumentNullException("commentId");
            CommentId = commentId;
        }

        public Guid CommentId { get; private set; }
    }
}
