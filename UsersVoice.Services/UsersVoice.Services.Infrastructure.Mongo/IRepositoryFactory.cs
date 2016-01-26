namespace UsersVoice.Infrastructure.Mongo
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> Create<TEntity>(RepositoryOptions options);
    }
}