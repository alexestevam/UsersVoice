using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using SimpleInjector;
using UsersVoice.Infrastructure.Mongo;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.API.CQRS.Mongo.Commands.Handlers;
using UsersVoice.Services.API.CQRS.Mongo.Events.Handlers;

namespace UsersVoice.TestDataFeeder
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container();
            RegisterInfrastructure(container);
            var assemblies = GetAssemblies().ToArray();
            container.RegisterSingleton<IMediator, Mediator>();
            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));
            container.RegisterCollection(typeof(IAsyncNotificationHandler<>), assemblies);
            container.Verify();

            var mediator = container.GetInstance<IMediator>();
            var commandsDbContext = container.GetInstance<ICommandsDbContext>();

            Console.Write("creating users...");
            Task.WaitAll(CreateUsersAsync(commandsDbContext, mediator));
            Console.WriteLine("done!");

            Console.Write("creating areas...");
            Task.WaitAll(CreateAreasAsync(commandsDbContext, mediator));
            Console.WriteLine("done!");

            Console.Write("creating ideas...");
            Task.WaitAll(CreateIdeasAsync(commandsDbContext, mediator));
            Console.WriteLine("done!");

            Console.Write("creating idea comments...");
            Task.WaitAll(CreateIdeaCommentsAsync(commandsDbContext, mediator));
            Console.WriteLine("done!");

            Console.WriteLine("Feeding complete!");
            Console.ReadLine();
        }

        private static async Task CreateUsersAsync(ICommandsDbContext commandsDbContext, IMediator bus)
        {
            var usersCount = await commandsDbContext.Users.CountAsync("{}");
            if (usersCount > 0)
                return;

            var firstNamesIndex = 0;
            var lastNamesIndex = 0;

            usersCount = 100;

            var firstNames =
                new string[]
                {
                    "John", "Paul", "Ringo", "Jack", "David", "Jim", "Johnny", "Brad", "Kim", "Jimmy", "Robert", "Freddy",
                    "Matthew", "Nicholas", "Mike"
                }.OrderBy(n => Guid.NewGuid()).ToArray();
            var lastNames =
                new string[]
                {
                    "Jones", "Mercury", "Starr", "Page", "Pitt", "Cage", "Bones", "Doe", "Bonham", "White", "Plant",
                    "Portnoy", "Grohl", "May"
                }.OrderBy(n => Guid.NewGuid()).ToArray();

            var commandHandler = new CreateUserCommandHandler(commandsDbContext, bus);

            var command = new CreateUser(Guid.NewGuid(), "admin", "admin", "admin@usersvoice.com", true);
            await commandHandler.Handle(command);

            command = new CreateUser(Guid.NewGuid(), "user", "user", "user@usersvoice.com");
            await commandHandler.Handle(command);

            for (int i = 0; i < usersCount; i++)
            {
                var firstName = firstNames[(firstNamesIndex++) % firstNames.Length];
                var lastName = lastNames[(lastNamesIndex++) % lastNames.Length];

                var username = string.Format("{0}.{1}", firstName, lastName).ToLower();
                var email = string.Format("{0}@email.com", username);
                if (await FindUserByEmailAsync(commandsDbContext.Users, email))
                {
                    var count = 0;
                    while (true)
                    {
                        email = string.Format("{0}{1}@email.com", username, ++count);
                        if (!await FindUserByEmailAsync(commandsDbContext.Users, email))
                            break;

                        if (count > 1000)
                        {
                            email = string.Empty;
                            break;
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(email))
                    continue;

                command = new CreateUser(Guid.NewGuid(), firstName, lastName, email);
                await commandHandler.Handle(command);
            }
        }

        private static async Task<bool> FindUserByEmailAsync(IRepository<User> usersColl, string email)
        {
            var foundUser = await usersColl.Find(u => u.Email == email).FirstOrDefaultAsync();
            return (null != foundUser);
        }

        private static async Task CreateAreasAsync(ICommandsDbContext commandsDbContext, IMediator bus)
        {
            var areasCount = await commandsDbContext.Ideas.CountAsync("{}");
            if (areasCount > 0)
                return;

            var users = await commandsDbContext.Users.Find("{}").Limit(100).ToListAsync();

            var usersIndex = 0;

            areasCount = 50;
            
            var commandHandler = new CreateAreaCommandHandler(commandsDbContext, bus);

            usersIndex = await CreateAreaAsync(users, usersIndex, commandHandler, "Canteen");
            usersIndex = await CreateAreaAsync(users, usersIndex, commandHandler, "Dell @ Limerick");
            usersIndex = await CreateAreaAsync(users, usersIndex, commandHandler, "Dell Sales Application");
            usersIndex = await CreateAreaAsync(users, usersIndex, commandHandler, "Employee Resource Group");
            usersIndex = await CreateAreaAsync(users, usersIndex, commandHandler, "Global Operations");
            usersIndex = await CreateAreaAsync(users, usersIndex, commandHandler, "IT");
            usersIndex = await CreateAreaAsync(users, usersIndex, commandHandler, "Sports & Social");
            usersIndex = await CreateAreaAsync(users, usersIndex, commandHandler, "Tech Central");
            //for (int i = 0; i != areasCount; ++i)
            //{
            //    var title = RandomTextGenerator.GetRandomSentence(5);
            //    usersIndex = await CreateAreaAsync(users, usersIndex, commandHandler, title);
            //}
        }

        private static async Task<int> CreateAreaAsync(List<User> users, int usersIndex, CreateAreaCommandHandler commandHandler, string title)
        {
            var author = users[usersIndex++ % users.Count()];

            var command = new CreateArea(Guid.NewGuid(), author.Id, title, RandomTextGenerator.GetRandomParagraph(3));
            await commandHandler.Handle(command);
            return usersIndex;
        }

        private static async Task CreateIdeasAsync(ICommandsDbContext commandsDbContext, IMediator bus)
        {
            var ideasCount = await commandsDbContext.Ideas.CountAsync("{}");
            if (ideasCount > 0)
                return;

            var users = await commandsDbContext.Users.Find("{}").Limit(100).ToListAsync();
            var areas = await commandsDbContext.Areas.Find("{}").ToListAsync();

            var usersIndex = 0;

            var commandHandler = new CreateIdeaCommandHandler(commandsDbContext, bus);

            foreach (var area in areas)
                for (int i = 0; i != 10; ++i)
                {
                    var author = users[usersIndex++ % users.Count()];

                    var command = new CreateIdea(Guid.NewGuid(), area.Id, author.Id, RandomTextGenerator.GetRandomSentence(5), RandomTextGenerator.GetRandomParagraph(3));
                    await commandHandler.Handle(command);
                }
        }

        private static async Task CreateIdeaCommentsAsync(ICommandsDbContext commandsDbContext, IMediator bus)
        {
            var commentsCount = await commandsDbContext.IdeaComments.CountAsync("{}");
            if (commentsCount > 0)
                return;

            var users = await commandsDbContext.Users.Find("{}").Limit(100).ToListAsync();
            var ideas = await commandsDbContext.Ideas.Find("{}").ToListAsync();

            var usersIndex = 0;

            var commandHandler = new CreateIdeaCommentCommandHandler(commandsDbContext, bus);

            foreach (var idea in ideas)
                for (int i = 0; i != 5; ++i)
                {
                    var author = users[usersIndex++ % users.Count()];

                    var command = new CreateIdeaComment(Guid.NewGuid(), idea.Id, author.Id, RandomTextGenerator.GetRandomSentence(20));
                    await commandHandler.Handle(command);
                }
        }


        private static void RegisterInfrastructure(Container container)
        {
            container.RegisterSingleton<IMongoDatabaseFactory, MongoDatabaseFactory>();
            container.RegisterSingleton<IRepositoryFactory, RepositoryFactory>();

            container.Register<ICommandsDbContext>(() =>
            {
                var repoFactory = container.GetInstance<IRepositoryFactory>();
                var connString = ConfigurationManager.ConnectionStrings["Commands"].ConnectionString;
                var dbName = "usersvoice_commands";
                return new CommandsDbContext(repoFactory, connString, dbName);
            });

            container.Register<IQueriesDbContext>(() =>
            {
                var repoFactory = container.GetInstance<IRepositoryFactory>();
                var connString = ConfigurationManager.ConnectionStrings["Queries"].ConnectionString;
                var dbName = "usersvoice_queries";
                return new QueriesDbContext(repoFactory, connString, dbName);
            });
        }


        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(IMediator).GetTypeInfo().Assembly;
            yield return typeof(IdeaCreatedEventHandler).GetTypeInfo().Assembly;
        }
    }
}
