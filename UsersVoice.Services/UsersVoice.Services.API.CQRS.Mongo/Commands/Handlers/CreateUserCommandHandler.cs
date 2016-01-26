using System;
using System.Threading.Tasks;
using MediatR;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.API.CQRS.Events;

namespace UsersVoice.Services.API.CQRS.Mongo.Commands.Handlers
{
    public class CreateUserCommandHandler : IAsyncNotificationHandler<CreateUser>
    { 
        private readonly ICommandsDbContext _commandsDb;
        private readonly IMediator _bus;

        public CreateUserCommandHandler(ICommandsDbContext commandsDb, IMediator bus)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (bus == null) throw new ArgumentNullException("bus");
            _commandsDb = commandsDb;
            _bus = bus;
        }

        public async Task Handle(CreateUser command)
        {
            if (null == command)
                throw new ArgumentNullException("command");
   
            var newUser = new User()
            {
                Id = command.UserId,
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                RegistrationDate = DateTime.Now,
                IsAdmin = command.IsAdmin,
                AvailablePoints = 10
            };
            await _commandsDb.Users.InsertOneAsync(newUser);

            await _bus.PublishAsync(new UserCreated(newUser.Id));
        }
    }
}
