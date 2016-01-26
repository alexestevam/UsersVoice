using System;
using MongoDB.Driver;

namespace UsersVoice.Infrastructure.Mongo.Queries
{
    public class MongoQueryDefinition<TEntity> : IMongoQueryDefinition<TEntity>
    {
        public MongoQueryDefinition(IRepository<TEntity> repository,
                                    FilterDefinition<TEntity> filter)
            : this(repository, filter, null) { }

        public MongoQueryDefinition(IRepository<TEntity> repository, 
                                    FilterDefinition<TEntity> filter,
                                    SortDefinition<TEntity> sorting)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            if (filter == null) throw new ArgumentNullException("filter");

            this.Repository = repository;
            this.Filter = filter;
            this.Sorting = sorting;
        }

        public IRepository<TEntity> Repository { get; private set; }

        public FilterDefinition<TEntity> Filter { get; private set; }

        public SortDefinition<TEntity> Sorting { get; private set; }
    }
}