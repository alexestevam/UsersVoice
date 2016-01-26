using System;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;

namespace UsersVoice.Infrastructure.Mongo.Commands
{
    public class CommandsDbContext : ICommandsDbContext
    {
        public CommandsDbContext(IRepositoryFactory repoFactory, string connectionString, string dbName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString");
            if (string.IsNullOrWhiteSpace(dbName))
                throw new ArgumentNullException("dbName");

            this.Users = repoFactory.Create<User>(new RepositoryOptions(connectionString, dbName, "users"));
            this.Ideas = repoFactory.Create<Idea>(new RepositoryOptions(connectionString, dbName, "ideas"));
            this.IdeaComments = repoFactory.Create<IdeaComment>(new RepositoryOptions(connectionString, dbName, "ideaComments"));
            this.Areas = repoFactory.Create<Area>(new RepositoryOptions(connectionString, dbName, "areas"));
        }

        public IRepository<User> Users { get; private set; }
        public IRepository<Idea> Ideas { get; private set; }
        public IRepository<IdeaComment> IdeaComments { get; private set; }
        public IRepository<Area> Areas { get; private set;  }
    }
}