using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.API.CQRS.Events;

namespace UsersVoice.Services.API.CQRS.Mongo.Commands.Handlers
{
    public class VoteIdeaCommandHandler : IAsyncNotificationHandler<VoteIdea>
    { 
        private readonly ICommandsDbContext _commandsDb;
        private readonly IMediator _bus;

        public VoteIdeaCommandHandler(ICommandsDbContext commandsDb, IMediator bus)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (bus == null) throw new ArgumentNullException("bus");
            _commandsDb = commandsDb;
            _bus = bus;
        }

        public async Task Handle(CQRS.Commands.VoteIdea command)
        {
            if (null == command)
                throw new ArgumentNullException("command");

            var idea = await _commandsDb.Ideas.Find(d => d.Id == command.IdeaId).FirstOrDefaultAsync();
            if (null == idea)
                throw new ArgumentException("invalid idea id: " + command.IdeaId);

            var voter = await _commandsDb.Users.Find(d => d.Id == command.VoterId).FirstOrDefaultAsync();
            if (null == voter)
                throw new ArgumentException("invalid voter id: " + command.VoterId);
            
            if (voter.AvailablePoints < command.Points)
                throw new ArgumentOutOfRangeException("not enough points available for user " + command.VoterId);

            var alreadyVoted = idea.Votes.Any(v => v.VoterId == voter.Id);
            if(alreadyVoted)
                throw new AccessViolationException("cannot vote twice!");

            idea.Votes.Add(new IdeaVote()
            {
                CreationDate = DateTime.Now,
                VoterId = voter.Id,
                Points = command.Points
            });

            await _commandsDb.Ideas.FindOneAndReplaceAsync(d => d.Id == command.IdeaId, idea);

            voter.AvailablePoints -= Math.Abs(command.Points);
            await _commandsDb.Users.FindOneAndReplaceAsync(u => u.Id == voter.Id, voter);

            await _bus.PublishAsync(new IdeaVoted(idea.Id, voter.Id));
        }
    }
}
