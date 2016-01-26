using System;
using System.Threading.Tasks;
using UsersVoice.Infrastructure.Mongo;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Queries.Runners
{
    public class DefaultDetailsQueryRunner<TEntity, TModel> : IQueryRunner<TModel>
    {
       private readonly IMongoQueryExecutor _mongoQueryExecutor;

        public DefaultDetailsQueryRunner(IMongoQueryExecutor mongoQueryExecutor)
        {
            if (mongoQueryExecutor == null) 
                throw new ArgumentNullException("mongoQueryExecutor");

            _mongoQueryExecutor = mongoQueryExecutor;
        }

        public async Task<TModel> RunAsync(IQueryDefinition filter)
        {
            var mongoFilter = filter as IMongoQueryDefinition<TEntity>;
            if (null == mongoFilter)
                throw new ArgumentException("queryDefinition has to be a valid instance of IMongoQueryDefinition");

            var entity = await _mongoQueryExecutor.ReadOneAsync(mongoFilter);
            if (null == entity) 
                return default(TModel);

            return AutoMapper.Mapper.Map<TEntity, TModel>(entity);
        }
    }
}