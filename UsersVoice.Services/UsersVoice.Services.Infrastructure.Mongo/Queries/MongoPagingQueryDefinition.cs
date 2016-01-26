using System;
using MongoDB.Driver;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Infrastructure.Mongo.Queries
{
    public class MongoPagingQueryDefinition<TEntity> : MongoQueryDefinition<TEntity>, IPagingInfo
    {
        public MongoPagingQueryDefinition(IRepository<TEntity> repository, 
                                          FilterDefinition<TEntity> filter,
                                          IPagingInfo pagingInfo)
            : this(repository, filter, pagingInfo, null) { }

        public MongoPagingQueryDefinition(IRepository<TEntity> repository, 
                                          FilterDefinition<TEntity> filter,
                                          IPagingInfo pagingInfo,
                                          SortDefinition<TEntity> sorting)
            : base(repository, filter, sorting)
        {
            if (pagingInfo == null) throw new ArgumentNullException("pagingInfo");

            this.Page = pagingInfo.Page;
            this.PageSize = pagingInfo.PageSize;
        }

        public int Page { get; private set; }

        public int PageSize { get; private set; }
    }
}