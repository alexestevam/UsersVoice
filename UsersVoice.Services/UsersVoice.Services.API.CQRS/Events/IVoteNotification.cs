using System;

namespace UsersVoice.Services.API.CQRS.Events
{
    public interface IVoteNotification
    {
        Guid IdeaId { get; }
        Guid VoterId { get; }
    }
}