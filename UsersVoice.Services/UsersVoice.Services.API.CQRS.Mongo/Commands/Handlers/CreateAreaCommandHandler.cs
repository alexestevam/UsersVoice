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
    public class CreateAreaCommandHandler : IAsyncNotificationHandler<CreateArea>
    { 
        private readonly ICommandsDbContext _commandsDb;
        private readonly IMediator _bus;

        public CreateAreaCommandHandler(ICommandsDbContext commandsDb, IMediator bus)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (bus == null) throw new ArgumentNullException("bus");
            _commandsDb = commandsDb;
            _bus = bus;
        }

        public async Task Handle(CreateArea command)
        {
            if (null == command)
                throw new ArgumentNullException("command");

            var author = await _commandsDb.Users.Find(d => d.Id == command.AuthorId).FirstOrDefaultAsync();
            if (null == author)
                throw new ArgumentException("invalid author id: " + command.AuthorId);
   
            var newArea = new Area()
            {
                Id = command.AreaId,
                AuthorId = author.Id,
                Title = command.Title,
                Description = command.Description,
                CreationDate = DateTime.Now
            };
            await _commandsDb.Areas.InsertOneAsync(newArea);

            await _bus.PublishAsync(new AreaCreated(newArea.Id));
        }
    }
}
