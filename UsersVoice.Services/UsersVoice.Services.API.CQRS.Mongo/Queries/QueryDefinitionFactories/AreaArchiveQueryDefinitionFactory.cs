using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;
using UsersVoice.Services.API.CQRS.Queries;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Queries.QueryDefinitionFactories
{
    public class AreasArchiveQueryDefinitionFactory : IQueryDefinitionFactory<AreasArchiveQuery>
    {
        private readonly IQueriesDbContext _db;

        public AreasArchiveQueryDefinitionFactory(IQueriesDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public IQueryDefinition Build(AreasArchiveQuery query)
        {
            if (null == query)
                throw new ArgumentNullException("query");

            var filters = new List<FilterDefinition<Area>>();
            var builder = Builders<Area>.Filter;

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                var expr = new BsonRegularExpression(new Regex(query.Title, RegexOptions.IgnoreCase));
                var titleFilter = Builders<Area>.Filter.Regex(vl => vl.Title, expr);
                filters.Add(titleFilter);
            }

            if (query.AuthorId != Guid.Empty)
            {
                var authorIdFilter = Builders<Area>.Filter.Eq(i => i.AuthorId, query.AuthorId);
                filters.Add(authorIdFilter);
            }

            var filter = new MongoPagingQueryDefinition<Area>(_db.Areas, builder.And(filters), query);
            return filter;
        }
    }
}