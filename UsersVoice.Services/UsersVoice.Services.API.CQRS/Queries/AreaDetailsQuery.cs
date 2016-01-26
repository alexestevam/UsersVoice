using System;
using MediatR;
using UsersVoice.Services.API.CQRS.Queries.Models;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Queries
{
    public class AreaDetailsQuery : IAsyncRequest<AreaDetails>, IQuery
    {
        public AreaDetailsQuery(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("id");
            this.Id = id;
        }

        public Guid Id{ get; private set; }
    }
}