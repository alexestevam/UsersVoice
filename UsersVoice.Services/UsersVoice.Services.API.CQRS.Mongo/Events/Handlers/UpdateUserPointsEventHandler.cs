using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Services.API.CQRS.Events;

namespace UsersVoice.Services.API.CQRS.Mongo.Events.Handlers
{
    public class UpdateUserPointsEventHandler : IAsyncNotificationHandler<CQRS.Events.IdeaVoted>, 
                                                IAsyncNotificationHandler<CQRS.Events.IdeaUnvoted>,
                                                IAsyncNotificationHandler<CQRS.Events.IdeaStatusChanged>
    {
        private readonly ICommandsDbContext _commandsDb;
        private readonly IQueriesDbContext _queryDb;

        public UpdateUserPointsEventHandler(ICommandsDbContext commandsDb, IQueriesDbContext queryDb)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (queryDb == null) throw new ArgumentNullException("queryDb");
            _commandsDb = commandsDb;
            _queryDb = queryDb;
        }

        public async Task Handle(IdeaStatusChanged @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var idea = await _commandsDb.Ideas.Find(d => d.Id == @event.IdeaId).FirstOrDefaultAsync();
            if (null == idea)
                throw new ArgumentException("invalid idea id: " + @event.IdeaId);

            if(null == idea.Votes || !idea.Votes.Any())
                return;
            foreach (var vote in idea.Votes)
            {
                try
                {
                    await UpdateUserPoints(idea, vote);
                }
                catch { }
            }            
        }

        private async Task UpdateUserPoints(UsersVoice.Infrastructure.Mongo.Commands.Entities.Idea idea, 
                                            UsersVoice.Infrastructure.Mongo.Commands.Entities.IdeaVote vote)
        {
            var srcUser = await _commandsDb.Users.Find(d => d.Id == vote.VoterId).FirstOrDefaultAsync();
            if (null == srcUser)
                throw new ArgumentException("invalid user id: " + vote.VoterId);

            var destUser = await _queryDb.Users.Find(d => d.Id == vote.VoterId).FirstOrDefaultAsync();
            if (null == destUser)
                throw new ArgumentException("invalid user id: " + vote.VoterId);

            var pointsToAdd = 0;
            if (idea.Status == UsersVoice.Infrastructure.Mongo.Commands.Entities.Idea.IdeaStatus.Cancelled)
                pointsToAdd = vote.Points;
            else if (idea.Status == UsersVoice.Infrastructure.Mongo.Commands.Entities.Idea.IdeaStatus.Approved)
                pointsToAdd = (int)Math.Ceiling((float)vote.Points * 1.5);
            else if (idea.Status == UsersVoice.Infrastructure.Mongo.Commands.Entities.Idea.IdeaStatus.Implemented)
                pointsToAdd = (int)Math.Ceiling((float)vote.Points * .5);

            if (0 == pointsToAdd)
                return;

            srcUser.AvailablePoints += pointsToAdd;
            await _commandsDb.Users.FindOneAndReplaceAsync(u => u.Id == srcUser.Id, srcUser);

            destUser.AvailablePoints += pointsToAdd;
            await _queryDb.Users.FindOneAndReplaceAsync(u => u.Id == destUser.Id, destUser);
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

            var srcUser = await _commandsDb.Users.Find(d => d.Id == @event.VoterId).FirstOrDefaultAsync();
            if (null == srcUser)
                throw new ArgumentException("invalid user id: " + @event.VoterId);

            var destUser = await _queryDb.Users.Find(d => d.Id == @event.VoterId).FirstOrDefaultAsync();
            if (null == destUser)
                throw new ArgumentException("invalid user id: " + @event.VoterId);

            destUser.AvailablePoints = srcUser.AvailablePoints;
            await _queryDb.Users.FindOneAndReplaceAsync(u => u.Id == destUser.Id, destUser);
        }
    }
}