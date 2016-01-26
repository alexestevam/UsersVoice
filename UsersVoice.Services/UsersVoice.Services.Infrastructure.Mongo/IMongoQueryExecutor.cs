using System.Threading.Tasks;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Infrastructure.Mongo
{
    public interface IMongoQueryExecutor
    {
        Task<TEntity> ReadOneAsync<TEntity>(IMongoQueryDefinition<TEntity> queryDefinition);

        Task<PagedCollection<TEntity>> ReadAsync<TEntity>(IMongoQueryDefinition<TEntity> queryDefinition);
    }
}