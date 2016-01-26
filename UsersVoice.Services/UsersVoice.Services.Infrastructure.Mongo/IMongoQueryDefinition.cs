using MongoDB.Driver;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Infrastructure.Mongo
{
    public interface IMongoQueryDefinition<TEntity> : IQueryDefinition
    {
        IRepository<TEntity> Repository { get; }

        FilterDefinition<TEntity> Filter { get; }

        SortDefinition<TEntity> Sorting { get; }
    }
}