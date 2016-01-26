using System;
using MediatR;
using UsersVoice.Services.API.CQRS.Queries.Models;
using UsersVoice.Services.Common.CQRS.Queries;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.API.CQRS.Queries
{
    public class AreasArchiveQuery : IAsyncRequest<PagedCollection<AreaArchiveItem>>, IQuery, IPagingInfo
    {
        public AreasArchiveQuery() : this(0, 10)
        {
        }

        public AreasArchiveQuery(int page, int pageSize)
        {
            this.Page = page;
            this.PageSize = pageSize;
        }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public string Title { get; set; }

        public Guid AuthorId { get; set; }
    }
}
