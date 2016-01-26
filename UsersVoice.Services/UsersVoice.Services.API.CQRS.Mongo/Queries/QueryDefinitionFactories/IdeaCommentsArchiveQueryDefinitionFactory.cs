using System;
using System.Collections.Generic;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;
using UsersVoice.Services.API.CQRS.Queries;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Queries.QueryDefinitionFactories
{
    public class IdeaCommentsArchiveQueryDefinitionFactory : IQueryDefinitionFactory<IdeaCommentsArchiveQuery>
    {
        private readonly IQueriesDbContext _db;

        public IdeaCommentsArchiveQueryDefinitionFactory(IQueriesDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public IQueryDefinition Build(IdeaCommentsArchiveQuery query)
        {
            if (null == query)
                throw new ArgumentNullException("query");

            var filters = new List<FilterDefinition<IdeaComment>>();
            var builder = Builders<IdeaComment>.Filter;

            if (query.IdeaId != Guid.Empty)
            {
                var ideaIdFilter = Builders<IdeaComment>.Filter.Eq(i => i.IdeaId, query.IdeaId);
                filters.Add(ideaIdFilter);
            }

            var filter = new MongoPagingQueryDefinition<IdeaComment>(_db.IdeaComments, builder.And(filters), query);
            return filter;
        }
    }
}