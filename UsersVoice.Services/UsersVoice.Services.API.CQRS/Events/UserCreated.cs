using System;
using MediatR;

namespace UsersVoice.Services.API.CQRS.Events
{
    public class UserCreated : IAsyncNotification
    {
        public UserCreated(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException("userId");
            UserId = userId;
        }
        public Guid UserId { get; private set; }
    }
}
