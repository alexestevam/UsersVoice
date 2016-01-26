using System;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Events.Handlers
{
    public class IdeaCreatedEventHandler : IAsyncNotificationHandler<CQRS.Events.IdeaCreated>
    {
        private readonly ICommandsDbContext _commandsDb;
        private readonly IQueriesDbContext _queryDb;

        public IdeaCreatedEventHandler(ICommandsDbContext commandsDb, IQueriesDbContext queryDb)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (queryDb == null) throw new ArgumentNullException("queryDb");
            _commandsDb = commandsDb;
            _queryDb = queryDb;
        }

        public async Task Handle(CQRS.Events.IdeaCreated @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var idea = await _commandsDb.Ideas.Find(d => d.Id == @event.IdeaId).FirstOrDefaultAsync();
            if (null == idea)
                throw new ArgumentException("invalid idea id: " + @event.IdeaId);

            var author = await _queryDb.Users.Find(d => d.Id == idea.AuthorId).FirstOrDefaultAsync();
            if (null == author)
                throw new ArgumentException("invalid author id: " + idea.AuthorId);

            var area = await _queryDb.Areas.Find(d => d.Id == idea.AreaId).FirstOrDefaultAsync();
            if (null == area)
                throw new ArgumentException("invalid area id: " + idea.AreaId);

            var newIdea = new UsersVoice.Infrastructure.Mongo.Queries.Entities.Idea()
            {
                Id = idea.Id,
                AuthorId = author.Id,
                AuthorCompleteName = author.CompleteName,
                CreationDate = idea.CreationDate,
                Title = idea.Title,
                Description = idea.Description,
                AreaId = area.Id,
                AreaTitle = area.Title
            };
            await _queryDb.Ideas.InsertOneAsync(newIdea);
        }
    }
}
