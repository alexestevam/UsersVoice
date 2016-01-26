using System;
using FluentAssertions;
using UsersVoice.Infrastructure.Mongo;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;
using Xunit;

namespace UsersVoice.Services.Infrastructure.Mongo.Tests
{
    public class RepositoryFactoryTests
    {
        [Fact]
        public void should_throw_ArgumentNullException_if_options_null()
        {
            Assert.Throws( typeof(ArgumentNullException), () => new RepositoryFactory(null) );
        }

        [Fact]
        public void should_create_repository()
        {
            var mockFactory = new FakeMongoDatabaseFactory();
            var sut = new RepositoryFactory(mockFactory);
            var result = sut.Create<User>(new RepositoryOptions("lorem", "ipsum", "users"));
            result.Should().NotBeNull();
            result.CollectionName.ShouldBeEquivalentTo("users");
        }
    }
}