using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace UsersVoice.Infrastructure.Mongo
{
    public class Repository<TEntity> : IRepository<TEntity>
    {
        private readonly IMongoCollection<TEntity> _collection;

        public Repository(IMongoCollection<TEntity> collection)
        {
            if (null == collection)
                throw new ArgumentNullException("collection");
            _collection = collection;

            this.CollectionName = collection.CollectionNamespace.CollectionName;
        }

        public string CollectionName { get; private set; }

        public Task<long> CountAsync(FilterDefinition<TEntity> filter)
        {
            return _collection.CountAsync(filter);
        }

        public IFindFluent<TEntity, TEntity> Find(FilterDefinition<TEntity> filter)
        {
            return _collection.Find(filter);
        }
        public IFindFluent<TEntity, TEntity> Find(Expression<Func<TEntity, bool>> filter)
        {
            return _collection.Find(filter);
        }

        public Task<TEntity> FindOneAndReplaceAsync(FilterDefinition<TEntity> filter, TEntity replacement)
        {
            return _collection.FindOneAndReplaceAsync(filter, replacement);
        }
        public Task<TEntity> FindOneAndReplaceAsync(Expression<Func<TEntity, bool>> filter, TEntity replacement)
        {
            return _collection.FindOneAndReplaceAsync(filter, replacement);
        }

        public Task InsertOneAsync(TEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }
    }
}