using System;
using MediatR;

namespace UsersVoice.Services.API.CQRS.Events
{
    public class IdeaUnvoted : IVoteNotification, IAsyncNotification
    {
        public IdeaUnvoted(Guid ideaId, Guid voterId)
        {
            if (ideaId == Guid.Empty) throw new ArgumentNullException("ideaId");
            if (voterId == Guid.Empty) throw new ArgumentNullException("voterId");
            IdeaId = ideaId;
            VoterId = voterId;
        }

        public Guid IdeaId { get; private set; }
        public Guid VoterId { get; private set; }
    }
}
