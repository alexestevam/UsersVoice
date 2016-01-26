using UsersVoice.Infrastructure.Mongo.Queries.Entities;

namespace UsersVoice.Infrastructure.Mongo.Queries
{
    public interface IQueriesDbContext
    {
        IRepository<User> Users { get; }
        IRepository<Idea> Ideas { get; }
        IRepository<IdeaComment> IdeaComments { get; }
        IRepository<Area> Areas { get; }
    }
}