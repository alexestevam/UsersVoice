using System;
using MediatR;

namespace UsersVoice.Services.API.CQRS.Commands
{
    public class UnvoteIdea : IAsyncNotification
    {
        public UnvoteIdea(Guid ideaId, Guid voterId)
        {
            this.IdeaId = ideaId;
            this.VoterId = voterId;
        }

        public Guid IdeaId { get; private set; }

        public Guid VoterId { get; private set; }
    }
}
