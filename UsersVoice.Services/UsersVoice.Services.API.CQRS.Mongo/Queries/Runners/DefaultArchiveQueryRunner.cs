using System;
using System.Linq;
using System.Threading.Tasks;
using UsersVoice.Infrastructure.Mongo;
using UsersVoice.Services.Common.CQRS.Queries;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.API.CQRS.Mongo.Queries.Runners
{
    public class DefaultArchiveQueryRunner<TEntity, TModel> : IQueryRunner<PagedCollection<TModel>>
    {
        private readonly IMongoQueryExecutor _mongoQueryExecutor;

        public DefaultArchiveQueryRunner(IMongoQueryExecutor mongoQueryExecutor)
        {
            if (mongoQueryExecutor == null) throw new ArgumentNullException("mongoQueryExecutor");
     
            _mongoQueryExecutor = mongoQueryExecutor;
        }

        public async Task<PagedCollection<TModel>> RunAsync(IQueryDefinition filter)
        {
            if (filter == null) throw new ArgumentNullException("filter");

            var mongoFilter = filter as IMongoQueryDefinition<TEntity>;
            if (null == mongoFilter)
                throw new ArgumentException("queryDefinition has to be a valid instance of IMongoQueryDefinition");

            var entities = await _mongoQueryExecutor.ReadAsync(mongoFilter);
            if (null == entities)
                return PagedCollection<TModel>.Empty;

            var items = entities.Items.Select(entity => AutoMapper.Mapper.Map<TEntity, TModel>(entity)).ToList();

            return new PagedCollection<TModel>(items, entities.Page, entities.PageSize, entities.TotalPagesCount, entities.TotalItemsCount);
        }
    }
}