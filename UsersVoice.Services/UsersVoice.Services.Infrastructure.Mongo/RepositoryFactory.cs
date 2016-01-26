using System;

namespace UsersVoice.Infrastructure.Mongo
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IMongoDatabaseFactory _dbFactory;

        public RepositoryFactory(IMongoDatabaseFactory dbFactory)
        {
            if (dbFactory == null) 
                throw new ArgumentNullException("dbFactory");
            _dbFactory = dbFactory;
        }

        public IRepository<TEntity> Create<TEntity>(RepositoryOptions options)
        {
            if (options == null) throw new ArgumentNullException("options");

            var db = _dbFactory.Connect(options.ConnectionString, options.DbName);
            return new Repository<TEntity>(db.GetCollection<TEntity>(options.CollectionName));
        }
    }
}