using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Services.API.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Queries.Handlers
{
    public class HasVotedQueryHandler : IAsyncRequestHandler<HasVotedQuery, bool>
    {
        private readonly IQueriesDbContext _db;

        public HasVotedQueryHandler(IQueriesDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public async Task<bool> Handle(HasVotedQuery query)
        {
            if (null == query)
                throw new ArgumentNullException("query");

            if (query.IdeaId == Guid.Empty)
                throw new ArgumentException("please provide an idea id");

            var idea = await _db.Ideas.Find(i => i.Id == query.IdeaId).FirstOrDefaultAsync();
            if (null == idea)
                throw new ArgumentException("invalid idea id: " + query.IdeaId);

            if (query.UserId == Guid.Empty)
                throw new ArgumentException("please provide a user id");

            var result = false;
            if (null != idea.Votes)
                result = idea.Votes.Any(v => v.VoterId == query.UserId);

            return result;
        }
    }
}