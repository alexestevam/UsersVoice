using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace UsersVoice.Infrastructure.Mongo
{
    public interface IRepository<TEntity>
    {
        string CollectionName { get; }

        Task<long> CountAsync(FilterDefinition<TEntity> filter);
        IFindFluent<TEntity, TEntity> Find(FilterDefinition<TEntity> filter);
        IFindFluent<TEntity, TEntity> Find(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> FindOneAndReplaceAsync(FilterDefinition<TEntity> filter, TEntity replacement);
        Task<TEntity> FindOneAndReplaceAsync(Expression<Func<TEntity, bool>> filter, TEntity replacement);

        Task InsertOneAsync(TEntity entity);
    }
}