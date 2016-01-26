using System;
using MediatR;

namespace UsersVoice.Services.API.CQRS.Events
{
    public class IdeaStatusChanged : IAsyncNotification
    {
        public IdeaStatusChanged(Guid ideaId)
        {
            if (ideaId == Guid.Empty) throw new ArgumentNullException("ideaId");
            IdeaId = ideaId;
        }

        public Guid IdeaId { get; private set; }
    }
}
