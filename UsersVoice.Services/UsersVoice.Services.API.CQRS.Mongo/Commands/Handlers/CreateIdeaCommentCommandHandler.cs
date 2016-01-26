using System;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.API.CQRS.Events;

namespace UsersVoice.Services.API.CQRS.Mongo.Commands.Handlers
{
    public class CreateIdeaCommentCommandHandler : IAsyncNotificationHandler<CreateIdeaComment>
    { 
        private readonly ICommandsDbContext _commandsDb;
        private readonly IMediator _bus;

        public CreateIdeaCommentCommandHandler(ICommandsDbContext commandsDb, IMediator bus)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (bus == null) throw new ArgumentNullException("bus");
            _commandsDb = commandsDb;
            _bus = bus;
        }

        public async Task Handle(CreateIdeaComment command)
        {
            if (null == command)
                throw new ArgumentNullException("command");

            var author = await _commandsDb.Users.Find(d => d.Id == command.AuthorId).FirstOrDefaultAsync();
            if (null == author)
                throw new ArgumentException("invalid author id: " + command.AuthorId);

            var idea = await _commandsDb.Ideas.Find(d => d.Id == command.IdeaId).FirstOrDefaultAsync();
            if (null == idea)
                throw new ArgumentException("invalid idea id: " + command.IdeaId);
   
            var newComment = new IdeaComment()
            {
                Id = command.CommentId,
                AuthorId = author.Id,
                IdeaId = idea.Id,
                Text = command.Text,
                CreationDate = DateTime.Now
            };
            await _commandsDb.IdeaComments.InsertOneAsync(newComment);

            await _bus.PublishAsync(new IdeaCommentCreated(newComment.Id));
        }
    }
}
