using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace UsersVoice.Services.Infrastructure.Mongo.Tests
{
    public class FakeFindFluent<TEntity> : IFindFluent<TEntity, TEntity>
    {
        private readonly IEnumerable<TEntity> _items;

        public FakeFindFluent(IEnumerable<TEntity> items)
        {
            _items = items ?? Enumerable.Empty<TEntity>();
        }

        public Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_items.FirstOrDefault());
        }

        public IFindFluent<TEntity, TResult> As<TResult>(
            MongoDB.Bson.Serialization.IBsonSerializer<TResult> resultSerializer = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult((long)_items.Count());
        }

        public FilterDefinition<TEntity> Filter
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IFindFluent<TEntity, TEntity> Limit(int? limit)
        {
            return this;
        }

        public FindOptions<TEntity, TEntity> Options
        {
            get { throw new NotImplementedException(); }
        }

        public IFindFluent<TEntity, TNewProjection> Project<TNewProjection>(
            ProjectionDefinition<TEntity, TNewProjection> projection)
        {
            throw new NotImplementedException();
        }

        public IFindFluent<TEntity, TEntity> Skip(int? skip)
        {
            return this;
        }

        public IFindFluent<TEntity, TEntity> Sort(SortDefinition<TEntity> sort)
        {
            return this;
        }

        public Task<IAsyncCursor<TEntity>> ToCursorAsync(CancellationToken cancellationToken)
        {
            IAsyncCursor<TEntity> cursor = new FakeAsyncCursor<TEntity>(_items);
            var task = Task.FromResult(cursor);

            return task;
        }


        public long Count(CancellationToken cancellationToken)
        {
            return _items.Count();
        }

        public IAsyncCursor<TEntity> ToCursor(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}