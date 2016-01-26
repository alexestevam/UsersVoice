using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;
using UsersVoice.Services.API.CQRS.Queries;
using UsersVoice.Services.Common.CQRS.Queries;

namespace UsersVoice.Services.API.CQRS.Mongo.Queries.QueryDefinitionFactories
{
    public class UsersArchiveQueryDefinitionFactory : IQueryDefinitionFactory<UsersArchiveQuery>
    {
        private readonly IQueriesDbContext _db;

        public UsersArchiveQueryDefinitionFactory(IQueriesDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public IQueryDefinition Build(UsersArchiveQuery query)
        {
            if(null == query)
                throw new ArgumentNullException("query");

            var filters = new List<FilterDefinition<User>>();
            var builder = Builders<User>.Filter;

            if (!string.IsNullOrWhiteSpace(query.CompleteName))
            {
                var expr = new BsonRegularExpression(new Regex(query.CompleteName, RegexOptions.IgnoreCase));
                var nameFilter = Builders<User>.Filter.Regex(vl => vl.CompleteName, expr);
                filters.Add(nameFilter);
            }

            if (!string.IsNullOrWhiteSpace(query.Email))
                filters.Add(builder.Eq(v => v.Email, query.Email));

            SortDefinition<User> sorting = null;
            if (query.SortBy != UserSortBy.None)
            {
                var sortingBuilder = Builders<User>.Sort;

                var sortingField = GetSortingField(query.SortBy);

                sorting = query.SortDirection == CQRS.Queries.SortDirection.ASC
                    ? sortingBuilder.Ascending(sortingField)
                    : sortingBuilder.Descending(sortingField);
            }

            var filter = new MongoPagingQueryDefinition<User>(_db.Users, builder.And(filters), query, sorting);
            return filter;
        }

        private static Expression<Func<User, object>> GetSortingField(UserSortBy sortBy)
        {
            switch (sortBy)
            {
                case UserSortBy.RegistrationDate:
                default:
                    return (User i) => i.RegistrationDate;
                case UserSortBy.Points:
                    return (User i) => i.AvailablePoints;
                case UserSortBy.Name:
                    return (User i) => i.LastName;
            }
        }
    }
}