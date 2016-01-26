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
    public class ChangeIdeaStatusCommandHandler : IAsyncNotificationHandler<CancelIdea>
    { 
        private readonly ICommandsDbContext _commandsDb;
        private readonly IMediator _bus;

        public ChangeIdeaStatusCommandHandler(ICommandsDbContext commandsDb, IMediator bus)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (bus == null) throw new ArgumentNullException("bus");
            _commandsDb = commandsDb;
            _bus = bus;
        }

        public async Task Handle(CQRS.Commands.CancelIdea command)
        {
            if (null == command)
                throw new ArgumentNullException("command");

            var idea = await _commandsDb.Ideas.Find(d => d.Id == command.IdeaId).FirstOrDefaultAsync();
            if (null == idea)
                throw new ArgumentException("invalid idea id: " + command.IdeaId);

            var user = await _commandsDb.Users.Find(d => d.Id == command.UserId).FirstOrDefaultAsync();
            if (null == user)
                throw new ArgumentException("invalid user id: " + command.UserId);
            
            if (!user.IsAdmin)
                throw new UnauthorizedAccessException("user is not authorized to execute this action");

            if (idea.Status == Idea.IdeaStatus.Cancelled)
                return;

            if (idea.Status != Idea.IdeaStatus.Nothing)
                throw new ArgumentException("invalid status");

            idea.Status = Idea.IdeaStatus.Cancelled;

            await _commandsDb.Ideas.FindOneAndReplaceAsync(d => d.Id == command.IdeaId, idea);

            await _bus.PublishAsync(new IdeaStatusChanged(idea.Id));
        }
    }
}
