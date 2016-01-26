using System;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Events.Handlers
{
    public class AreaCreatedEventHandler : IAsyncNotificationHandler<CQRS.Events.AreaCreated>
    {
        private readonly ICommandsDbContext _commandsDb;
        private readonly IQueriesDbContext _queryDb;

        public AreaCreatedEventHandler(ICommandsDbContext commandsDb, IQueriesDbContext queryDb)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (queryDb == null) throw new ArgumentNullException("queryDb");
            _commandsDb = commandsDb;
            _queryDb = queryDb;
        }

        public async Task Handle(CQRS.Events.AreaCreated @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var area = await _commandsDb.Areas.Find(d => d.Id == @event.AreaId).FirstOrDefaultAsync();
            if (null == area)
                throw new ArgumentException("invalid area id: " + @event.AreaId);

            var author = await _queryDb.Users.Find(d => d.Id == area.AuthorId).FirstOrDefaultAsync();
            if (null == author)
                throw new ArgumentException("invalid author id: " + area.AuthorId);

            var newArea = new UsersVoice.Infrastructure.Mongo.Queries.Entities.Area()
            {
                Id = area.Id,
                AuthorId = author.Id,
                AuthorCompleteName = author.CompleteName,
                CreationDate = area.CreationDate,
                Title = area.Title,
                Description = area.Description
            };
            await _queryDb.Areas.InsertOneAsync(newArea);
        }
    }
}
