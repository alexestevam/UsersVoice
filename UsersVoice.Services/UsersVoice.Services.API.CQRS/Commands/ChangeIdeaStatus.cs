using System;
using MediatR;

namespace UsersVoice.Services.API.CQRS.Commands
{
    public abstract class ChangeIdeaStatus : IAsyncNotification
    {
        public ChangeIdeaStatus(Guid ideaId, Guid userId)
        {
            if (ideaId == Guid.Empty) throw new ArgumentNullException("ideaId");
            if (userId == Guid.Empty) throw new ArgumentNullException("userId");
            this.IdeaId = ideaId;
            this.UserId = userId;            
        }

        public Guid IdeaId { get; private set; }

        public Guid UserId { get; private set; }
    }

    public class CancelIdea : ChangeIdeaStatus
    {
        public CancelIdea(Guid ideaId, Guid userId) : base(ideaId, userId) { }
    }

    public class ApproveIdea : ChangeIdeaStatus
    {
        public ApproveIdea(Guid ideaId, Guid userId) : base(ideaId, userId) { }
    }

    public class ImplementIdea : ChangeIdeaStatus
    {
        public ImplementIdea(Guid ideaId, Guid userId) : base(ideaId, userId) { }
    }
}
