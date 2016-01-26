using System;
using MediatR;
using UsersVoice.Services.API.CQRS.Queries.Models;
using UsersVoice.Services.Common.CQRS.Queries;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.API.CQRS.Queries
{
    public class IdeasArchiveQuery : IAsyncRequest<PagedCollection<IdeaArchiveItem>>, IQuery, IPagingInfo
    {
        public IdeasArchiveQuery() : this(0, 10)
        {
        }

        public IdeasArchiveQuery(int page, int pageSize)
        {
            this.Page = page;
            this.PageSize = pageSize;
        }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public string Title { get; set; }

        public Guid AuthorId { get; set; }

        public Guid AreaId { get; set; }

        public IdeaStatusQuery Status { get; set; }

        public IdeaSortBy SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
