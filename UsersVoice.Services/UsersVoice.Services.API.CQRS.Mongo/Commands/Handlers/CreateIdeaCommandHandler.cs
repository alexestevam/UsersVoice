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
    public class CreateIdeaCommandHandler : IAsyncNotificationHandler<CreateIdea>
    { 
        private readonly ICommandsDbContext _commandsDb;
        private readonly IMediator _bus;

        public CreateIdeaCommandHandler(ICommandsDbContext commandsDb, IMediator bus)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (bus == null) throw new ArgumentNullException("bus");
            _commandsDb = commandsDb;
            _bus = bus;
        }

        public async Task Handle(CreateIdea command)
        {
            if (null == command)
                throw new ArgumentNullException("command");

            var author = await _commandsDb.Users.Find(d => d.Id == command.AuthorId).FirstOrDefaultAsync();
            if (null == author)
                throw new ArgumentException("invalid author id: " + command.AuthorId);

            var area = await _commandsDb.Areas.Find(d => d.Id == command.AreaId).FirstOrDefaultAsync();
            if (null == area)
                throw new ArgumentException("invalid area id: " + command.AreaId);
   
            var newIdea = new Idea()
            {
                Id = command.IdeaId,
                AreaId = command.AreaId,
                AuthorId = author.Id,
                Title = command.Title,
                Description = command.Description,
                CreationDate = DateTime.Now
            };
            await _commandsDb.Ideas.InsertOneAsync(newIdea);

            await _bus.PublishAsync(new IdeaCreated(newIdea.Id));
        }
    }
}
