using System;
using MediatR;
using UsersVoice.Services.API.CQRS.Queries.Models;
using UsersVoice.Services.Common.CQRS.Queries;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.API.CQRS.Queries
{
    public class IdeaCommentsArchiveQuery : IAsyncRequest<PagedCollection<IdeaCommentArchiveItem>>, IQuery, IPagingInfo
    {
        public IdeaCommentsArchiveQuery() : this(0, 10)
        {
        }

        public IdeaCommentsArchiveQuery(int page, int pageSize)
        {
            this.Page = page;
            this.PageSize = pageSize;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public Guid IdeaId { get; set; }
    }
}
