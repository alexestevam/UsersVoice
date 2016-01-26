using System;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;
using UsersVoice.Services.API.CQRS.Queries;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Queries.QueryDefinitionFactories
{
    public class UserDetailsQueryDefinitionFactory : IQueryDefinitionFactory<UserDetailsQuery>
    {
        private readonly IQueriesDbContext _db;

        public UserDetailsQueryDefinitionFactory(IQueriesDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public IQueryDefinition Build(UserDetailsQuery query)
        {
            if (null == query)
                throw new ArgumentNullException("query");
            if(query.Id == Guid.Empty)
                throw new ArgumentException("please provide a user id");

            var builder = Builders<User>.Filter;
            var filter = builder.Eq(v => v.Id, query.Id);

            var queryDef = new MongoQueryDefinition<User>(_db.Users, filter);
            return queryDef;
        }
    }
}