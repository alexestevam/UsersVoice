using FluentAssertions;
using UsersVoice.Infrastructure.Mongo;
using Xunit;

namespace UsersVoice.Services.Infrastructure.Mongo.Tests
{
    public class RepositoryOptionsTests
    {
        [Fact]
        public void properties_should_be_valid()
        {
            var sut = new RepositoryOptions("lorem", "ipsum", "dolor");
            sut.ConnectionString.ShouldBeEquivalentTo("lorem");
            sut.DbName.ShouldBeEquivalentTo("ipsum");
            sut.CollectionName.ShouldBeEquivalentTo("dolor");
        }
    }
}