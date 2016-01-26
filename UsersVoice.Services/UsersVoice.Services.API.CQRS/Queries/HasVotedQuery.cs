using System;
using MediatR;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Queries
{
    public class HasVotedQuery : IAsyncRequest<bool>, IQuery
    {
        public HasVotedQuery(Guid userId, Guid ideaId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException("userId");
            if (ideaId == Guid.Empty)
                throw new ArgumentNullException("ideaId");

            this.UserId = userId;
            this.IdeaId = ideaId;
        }

        public Guid UserId{ get; private set; }
        public Guid IdeaId { get; private set; }
    }
}