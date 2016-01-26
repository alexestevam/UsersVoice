using System;

namespace UsersVoice.Infrastructure.Mongo
{
    public class RepositoryOptions
    {
        public RepositoryOptions(string connectionString, string dbName, string collectionName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString");
            if (string.IsNullOrWhiteSpace(dbName))
                throw new ArgumentNullException("dbName");
            if (string.IsNullOrWhiteSpace(collectionName))
                throw new ArgumentNullException("collectionName");

            CollectionName = collectionName;
            DbName = dbName;
            ConnectionString = connectionString;
        }

        public readonly string ConnectionString;
        public readonly string DbName;
        public readonly string CollectionName;
    }
}