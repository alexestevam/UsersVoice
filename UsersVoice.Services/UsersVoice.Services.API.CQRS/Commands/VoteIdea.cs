using System;
using MediatR;

namespace UsersVoice.Services.API.CQRS.Commands
{
    public class VoteIdea : IAsyncNotification
    {
        public VoteIdea(Guid ideaId, Guid voterId, short points)
        {
            this.IdeaId = ideaId;
            this.VoterId = voterId;
            this.Points = points;
        }

        public Guid IdeaId { get; private set; }

        public Guid VoterId { get; private set; }

        public short Points { get; private set; }
    }
}
