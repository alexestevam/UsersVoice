using System;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Events.Handlers
{
    public class IdeaCommentCreatedEventHandler : IAsyncNotificationHandler<CQRS.Events.IdeaCommentCreated>
    {
        private readonly ICommandsDbContext _commandsDb;
        private readonly IQueriesDbContext _queryDb;

        public IdeaCommentCreatedEventHandler(ICommandsDbContext commandsDb, IQueriesDbContext queryDb)
        {
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            if (queryDb == null) throw new ArgumentNullException("queryDb");
            _commandsDb = commandsDb;
            _queryDb = queryDb;
        }

        public async Task Handle(CQRS.Events.IdeaCommentCreated @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            var comment = await _commandsDb.IdeaComments.Find(d => d.Id == @event.CommentId).FirstOrDefaultAsync();
            if (null == comment)
                throw new ArgumentException("invalid comment id: " + @event.CommentId);

            var author = await _queryDb.Users.Find(d => d.Id == comment.AuthorId).FirstOrDefaultAsync();
            if (null == author)
                throw new ArgumentException("invalid author id: " + comment.AuthorId);

            var idea = await _queryDb.Ideas.Find(d => d.Id == comment.IdeaId).FirstOrDefaultAsync();
            if (null == idea)
                throw new ArgumentException("invalid idea id: " + comment.IdeaId);

            var newComment = new UsersVoice.Infrastructure.Mongo.Queries.Entities.IdeaComment()
            {
                Id = comment.Id,
                AuthorId = author.Id,
                AuthorCompleteName = author.CompleteName,
                CreationDate = idea.CreationDate,
                Text = comment.Text,
                IdeaId = idea.Id
            };
            await _queryDb.IdeaComments.InsertOneAsync(newComment);

            idea.TotalComments++;
            await _queryDb.Ideas.FindOneAndReplaceAsync(i => i.Id == idea.Id, idea);
        }
    }
}
