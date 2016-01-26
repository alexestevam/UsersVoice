using MongoDB.Driver;
using Moq;
using UsersVoice.Infrastructure.Mongo;

namespace UsersVoice.Services.Infrastructure.Mongo.Tests
{
    public class FakeMongoDatabaseFactory : IMongoDatabaseFactory
    {
        public MongoDB.Driver.IMongoDatabase Connect(string connectionString, string dbName)
        {
            var dbMock = new Mock<IMongoDatabase>();

            SetupCommandEntities(dbMock);

            SetupQueryEntities(dbMock);

            return dbMock.Object;
        }

        private void SetupQueryEntities(Mock<IMongoDatabase> dbMock)
        {
            SetupEntity<UsersVoice.Infrastructure.Mongo.Queries.Entities.User>(dbMock, "users");
            SetupEntity<UsersVoice.Infrastructure.Mongo.Queries.Entities.Idea>(dbMock, "ideas");
            SetupEntity<UsersVoice.Infrastructure.Mongo.Queries.Entities.Area>(dbMock, "areas");
            SetupEntity<UsersVoice.Infrastructure.Mongo.Queries.Entities.IdeaComment>(dbMock, "ideaComments");
        }

        private void SetupCommandEntities(Mock<IMongoDatabase> dbMock)
        {
            SetupEntity<UsersVoice.Infrastructure.Mongo.Commands.Entities.User>(dbMock, "users");
            SetupEntity<UsersVoice.Infrastructure.Mongo.Commands.Entities.Idea>(dbMock, "ideas");
            SetupEntity<UsersVoice.Infrastructure.Mongo.Commands.Entities.Area>(dbMock, "areas");
            SetupEntity<UsersVoice.Infrastructure.Mongo.Commands.Entities.IdeaComment>(dbMock, "ideaComments");
        }

        private void SetupEntity<TEntity>(Mock<IMongoDatabase> dbMock, string collectionName)
        {
            dbMock.Setup(d => d.GetCollection<TEntity>(collectionName, null)).Returns(MockCollection<TEntity>(collectionName));
        }

        private IMongoCollection<TDoc> MockCollection<TDoc>(string collName)
        {
            var mockColl = new Mock<IMongoCollection<TDoc>>();
            mockColl.SetupGet(c => c.CollectionNamespace).Returns(new CollectionNamespace("videoportal", collName));

            return mockColl.Object;
        }
    }
}