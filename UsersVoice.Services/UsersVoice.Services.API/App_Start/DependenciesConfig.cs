using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using MediatR;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using UsersVoice.Infrastructure.Mongo;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.API.CQRS.Mongo.Commands.Handlers;
using UsersVoice.Services.API.CQRS.Mongo.Events.Handlers;
using UsersVoice.Services.API.CQRS.Mongo.Queries.QueryDefinitionFactories;
using UsersVoice.Services.API.CQRS.Mongo.Queries.Runners;
using UsersVoice.Services.API.CQRS.Queries;
using UsersVoice.Services.API.CQRS.Queries.Handlers;
using UsersVoice.Services.API.CQRS.Queries.Models;
using UsersVoice.Services.Common.CQRS.Queries;
using UsersVoice.Services.Infrastructure.Common;

namespace UsersVoice.Services.API
{
    public class DependenciesConfig
    {
        public static void Register(HttpConfiguration config)
        {
            RegisterMappers();

            RegisterDependencies(config);
        }

        private static void RegisterDependencies(HttpConfiguration config)
        {
            var container = new Container();
            container.RegisterWebApiControllers(config);

            RegisterInfrastructure(container);

            var assemblies = GetAssemblies().ToArray();
            container.Register(typeof (IAsyncRequestHandler<,>), assemblies);
            container.RegisterCollection(typeof (IAsyncNotificationHandler<>), assemblies);

            RegisterHandlers(container);

            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
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

            RegisterMediator(container);
        }

        private static void RegisterMediator(Container container)
        {
            container.RegisterSingleton<IMediator, Mediator>();

            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));
        }

        private static void RegisterMappers()
        {
            AutoMapper.Mapper.CreateMap<User, UserArchiveItem>();
            AutoMapper.Mapper.CreateMap<User, UserDetails>();

            AutoMapper.Mapper.CreateMap<Area, AreaArchiveItem>();
            AutoMapper.Mapper.CreateMap<Area, AreaDetails>();

            AutoMapper.Mapper.CreateMap<Idea, IdeaArchiveItem>();
            AutoMapper.Mapper.CreateMap<Idea, IdeaDetails>();

            AutoMapper.Mapper.CreateMap<IdeaComment, IdeaCommentArchiveItem>();
        }

        #region Handlers

        private static void RegisterHandlers(Container container)
        {
            container.RegisterSingleton<IMongoQueryExecutor, MongoQueryExecutor>();

            RegisterUserHandlers(container);
            RegisterAreaHandlers(container);
            RegisterIdeaHandlers(container);
        }

        private static void RegisterUserHandlers(Container container)
        {
            RegisterArchiveHandlers<User, UsersArchiveQueryDefinitionFactory, UsersArchiveQuery, UserArchiveItem>(container);
            RegisterDetailsHandlers<User, UserDetailsQueryDefinitionFactory, UserDetailsQuery, UserDetails>(container);
        }

        private static void RegisterAreaHandlers(Container container)
        {
            RegisterArchiveHandlers<Area, AreasArchiveQueryDefinitionFactory, AreasArchiveQuery, AreaArchiveItem>(container);
            RegisterDetailsHandlers<Area, AreaDetailsQueryDefinitionFactory, AreaDetailsQuery, AreaDetails>(container);
        }

        private static void RegisterIdeaHandlers(Container container)
        {
            RegisterArchiveHandlers<Idea, IdeasArchiveQueryDefinitionFactory, IdeasArchiveQuery, IdeaArchiveItem>(container);
            RegisterDetailsHandlers<Idea, IdeaDetailsQueryDefinitionFactory, IdeaDetailsQuery, IdeaDetails>(container);

            container.Register<IAsyncNotificationHandler<VoteIdea>, VoteIdeaCommandHandler>();

            RegisterArchiveHandlers<IdeaComment, IdeaCommentsArchiveQueryDefinitionFactory, IdeaCommentsArchiveQuery, IdeaCommentArchiveItem>(container);
        }

        private static void RegisterArchiveHandlers<TEntity, TQueryDefFactory, TQuery, TResult>(Container container)
            where TQueryDefFactory : class, IQueryDefinitionFactory<TQuery>
            where TQuery : MediatR.IAsyncRequest<PagedCollection<TResult>>, IQuery
        {
            container.Register<IQueryDefinitionFactory<TQuery>, TQueryDefFactory>();
            container.Register<IQueryRunner<PagedCollection<TResult>>, DefaultArchiveQueryRunner<TEntity, TResult>>();
            container.Register<IAsyncRequestHandler<TQuery, PagedCollection<TResult>>,
                DefaultArchiveQueryHandler<TQuery, TResult>>();
        }

        private static void RegisterDetailsHandlers<TEntity, TQueryDefFactory, TQuery, TResult>(Container container)
            where TQueryDefFactory : class, IQueryDefinitionFactory<TQuery>
            where TQuery : MediatR.IAsyncRequest<TResult>, IQuery
        {
            container.Register<IQueryDefinitionFactory<TQuery>, TQueryDefFactory>();
            container.Register<IQueryRunner<TResult>, DefaultDetailsQueryRunner<TEntity, TResult>>();
            container.Register<IAsyncRequestHandler<TQuery, TResult>,
                DefaultDetailsQueryHandler<TQuery, TResult>>();
        }

        #endregion Handlers

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(IMediator).GetTypeInfo().Assembly;
            yield return typeof(DefaultArchiveQueryHandler<,>).GetTypeInfo().Assembly;
            yield return typeof(IdeaCreatedEventHandler).GetTypeInfo().Assembly;
        }
    }
}