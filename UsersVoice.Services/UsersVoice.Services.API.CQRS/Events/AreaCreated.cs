using System;
using MediatR;

namespace UsersVoice.Services.API.CQRS.Events
{
    public class AreaCreated : IAsyncNotification
    {
        public AreaCreated(Guid areaId)
        {
            if (areaId == Guid.Empty) throw new ArgumentNullException("areaId");
            AreaId = areaId;
        }
        public Guid AreaId { get; private set; }
    }
}
