using FluentAssertions;
using UsersVoice.Infrastructure.Mongo;
using UsersVoice.Infrastructure.Mongo.Queries;
using Xunit;

namespace UsersVoice.Services.Infrastructure.Mongo.Tests.Queries
{
    public class QueriesDbContextTests
    {
        [Fact]
        public void collections_should_be_initialized()
        {
            var mockFactory = new FakeMongoDatabaseFactory();
            var repoFactory = new RepositoryFactory(mockFactory);
            var sut = new QueriesDbContext(repoFactory, "lorem", "ipsum");

            sut.Users.Should().NotBeNull();
            sut.Users.CollectionName.ShouldBeEquivalentTo("users");

            sut.Areas.Should().NotBeNull();
            sut.Areas.CollectionName.ShouldBeEquivalentTo("areas");

            sut.Ideas.Should().NotBeNull();
            sut.Ideas.CollectionName.ShouldBeEquivalentTo("ideas");

            sut.IdeaComments.Should().NotBeNull();
            sut.IdeaComments.CollectionName.ShouldBeEquivalentTo("ideaComments");
        }
    }
}
