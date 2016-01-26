using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MediatR;
using Moq;
using UsersVoice.Infrastructure.Mongo;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;
using UsersVoice.Services.API.CQRS.Mongo.Commands.Handlers;
using UsersVoice.Services.Infrastructure.Mongo.Tests;
using Xunit;

namespace UsersVoice.Services.API.CQRS.Mongo.Tests.Commands.Handlers
{
    public class VoteIdeaCommandHandlerTests
    {
        private readonly VoteIdeaCommandHandler _sut;
        private readonly Mock<IRepository<User>> _mockUsersRepo;
        private readonly Mock<IRepository<Idea>> _mockIdeasRepo;

        public VoteIdeaCommandHandlerTests()
        {
            _mockIdeasRepo = new Mock<IRepository<Idea>>();
            _mockIdeasRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Idea, bool>>>()))
                .Returns(new FakeFindFluent<Idea>(null));

            _mockUsersRepo = new Mock<IRepository<User>>();
            _mockUsersRepo.Setup(r => r.Find(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(new FakeFindFluent<User>(null));

            var mockDbContext = new Mock<ICommandsDbContext>();
            mockDbContext.SetupGet(c => c.Ideas).Returns(_mockIdeasRepo.Object);
            mockDbContext.SetupGet(c => c.Users).Returns(_mockUsersRepo.Object);

            _sut = new VoteIdeaCommandHandler(mockDbContext.Object, new Mediator(null, null));
        }

        [Fact]
        public async Task should_throw_if_null_command()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.Handle(null));
        }
    }
}
