using System;
using MediatR;

namespace UsersVoice.Services.API.CQRS.Commands
{
    public class CreateIdeaComment : IAsyncNotification
    {
        public CreateIdeaComment(Guid commentId, Guid ideaId, Guid authorId, string text)
        {
            if (commentId == Guid.Empty) throw new ArgumentNullException("commentId");
            if (ideaId == Guid.Empty) throw new ArgumentNullException("ideaId");
            if (authorId == Guid.Empty) throw new ArgumentNullException("authorId");
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException("text");

            this.Text = text;

            this.AuthorId = authorId;
            
            this.IdeaId = ideaId;

            this.CommentId = commentId;
        }

        public string Text { get; private set; }
        public Guid AuthorId { get; private set; }
        public Guid IdeaId { get; private set; }
        public Guid CommentId { get; private set; }
    }
}
