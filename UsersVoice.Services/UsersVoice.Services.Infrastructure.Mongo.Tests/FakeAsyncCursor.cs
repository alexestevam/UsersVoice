using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace UsersVoice.Services.Infrastructure.Mongo.Tests
{
    public class FakeAsyncCursor<TEntity> : IAsyncCursor<TEntity>
    {
        private readonly IEnumerable<TEntity> _items;

        public FakeAsyncCursor(IEnumerable<TEntity> items)
        {
            _items = items ?? Enumerable.Empty<TEntity>();
        }

        public IEnumerable<TEntity> Current
        {
            get { return _items; }
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_items.GetEnumerator().MoveNext());
        }

        public void Dispose(){ }


        public bool MoveNext(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}