using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;
using UsersVoice.Services.API.CQRS.Events;

namespace UsersVoice.Services.API.CQRS.Mongo.Events.Handlers
{
    public class UpdateIdeaVotesEventHandler : IAsyncNotificationHandler<CQRS.Events.IdeaVoted>
                                             , IAsyncNotificationHandler<CQRS.Events.IdeaUnvoted>
    {
        private readonly ICommandsDbContext _commandsDb;
        private readonly IQueriesDbContext _queryDb;

        public UpdateIdeaVotesEventHandler(ICommandsDbContext commandsDb, IQueriesDbContext queryDb)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (queryDb == null) throw new ArgumentNullException("queryDb");
            _commandsDb = commandsDb;
            _queryDb = queryDb;
        }

        public async Task Handle(CQRS.Events.IdeaVoted @event)
        {
            await UpdateData(@event);
        }
        public async Task Handle(CQRS.Events.IdeaUnvoted @event)
        {
            await UpdateData(@event);
        }

        private async Task UpdateData(IVoteNotification @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var srcIdea = await _commandsDb.Ideas.Find(d => d.Id == @event.IdeaId).FirstOrDefaultAsync();
            if (null == srcIdea)
                throw new ArgumentException("invalid idea id: " + @event.IdeaId);

            var destIdea = await _queryDb.Ideas.Find(d => d.Id == @event.IdeaId).FirstOrDefaultAsync();
            if (null == destIdea)
                throw new ArgumentException("unable to find idea in query database: " + @event.IdeaId);

            destIdea.Votes.Clear();

            foreach (var vote in srcIdea.Votes)
            {
                var voter = await _queryDb.Users.Find(d => d.Id == vote.VoterId).FirstOrDefaultAsync();
                if (null == voter)
                    throw new ArgumentException("invalid voter id: " + vote.VoterId);

                destIdea.Votes.Add(new IdeaVote()
                {
                    CreationDate = vote.CreationDate,
                    Points = vote.Points,
                    VoterId = vote.VoterId,
                    VoterCompleteName = voter.CompleteName
                });
            }

            destIdea.TotalPoints = destIdea.Votes.Sum(iv => iv.Points);

            await _queryDb.Ideas.FindOneAndReplaceAsync(d => d.Id == destIdea.Id, destIdea);
        }

    }
}
