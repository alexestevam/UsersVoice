using System;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Events.Handlers
{
    public class IdeaStatusChangedCommandHandler : IAsyncNotificationHandler<CQRS.Events.IdeaStatusChanged>
    {
        private readonly ICommandsDbContext _commandsDb;
        private readonly IQueriesDbContext _queryDb;

        public IdeaStatusChangedCommandHandler(ICommandsDbContext commandsDb, IQueriesDbContext queryDb)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (queryDb == null) throw new ArgumentNullException("queryDb");
            _commandsDb = commandsDb;
            _queryDb = queryDb;
        }

        public async Task Handle(CQRS.Events.IdeaStatusChanged @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var srcIdea = await _commandsDb.Ideas.Find(d => d.Id == @event.IdeaId).FirstOrDefaultAsync();
            if (null == srcIdea)
                throw new ArgumentException("invalid idea id: " + @event.IdeaId);

            var destIdea = await _queryDb.Ideas.Find(d => d.Id == @event.IdeaId).FirstOrDefaultAsync();
            if (null == destIdea)
                throw new ArgumentException("cannot find idea query model, id: " + @event.IdeaId);

            destIdea.Status = (UsersVoice.Infrastructure.Mongo.Queries.Entities.Idea.IdeaStatus) srcIdea.Status;

            await _queryDb.Ideas.FindOneAndReplaceAsync(i=> i.Id == destIdea.Id, destIdea);
        }
    }
}
