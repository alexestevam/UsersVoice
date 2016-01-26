using MediatR;
using UsersVoice.Services.API.CQRS.Queries.Models;
using UsersVoice.Services.Common.CQRS.Queries;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.API.CQRS.Queries
{
    public class UsersArchiveQuery : IAsyncRequest<PagedCollection<UserArchiveItem>>, IQuery, IPagingInfo
    {
        public UsersArchiveQuery() : this(0, 10) { }
        public UsersArchiveQuery(int page, int pageSize)
        {
            this.Page = page;
            this.PageSize = pageSize;
        }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public string CompleteName { get; set; }
        
        public string Email{ get; set; }

        public UserSortBy SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}