using MongoDB.Driver;

namespace UsersVoice.Infrastructure.Mongo
{
    public interface IMongoDatabaseFactory
    {
        IMongoDatabase Connect(string connectionString, string dbName);
    }
}