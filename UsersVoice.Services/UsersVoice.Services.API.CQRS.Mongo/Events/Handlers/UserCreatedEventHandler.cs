using System;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Events.Handlers
{
    public class UserCreatedEventHandler : IAsyncNotificationHandler<CQRS.Events.UserCreated>
    {
        private readonly ICommandsDbContext _commandsDb;
        private readonly IQueriesDbContext _queryDb;
        
        public UserCreatedEventHandler(ICommandsDbContext commandsDb, IQueriesDbContext queryDb)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (queryDb == null) throw new ArgumentNullException("queryDb");
            
            _commandsDb = commandsDb;
            _queryDb = queryDb;
        }

        public async Task Handle(CQRS.Events.UserCreated @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var srcUser = await _commandsDb.Users.Find(d => d.Id == @event.UserId).FirstOrDefaultAsync();
            if (null == srcUser)
                throw new ArgumentException("invalid user id: " + @event.UserId);

            var newUser = new UsersVoice.Infrastructure.Mongo.Queries.Entities.User()
            {
                Id = srcUser.Id,
                Email = srcUser.Email,
                FirstName = srcUser.FirstName,
                LastName = srcUser.LastName,
                RegistrationDate = DateTime.Now,
                IsAdmin = srcUser.IsAdmin,
                AvailablePoints = srcUser.AvailablePoints
            };
            await _queryDb.Users.InsertOneAsync(newUser);
        }
    }
}
