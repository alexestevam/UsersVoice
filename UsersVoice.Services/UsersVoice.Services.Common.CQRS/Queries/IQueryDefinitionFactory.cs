namespace UsersVoice.Services.Common.CQRS.Queries
{
    public interface IQueryDefinitionFactory<in TQuery>
        where TQuery : IQuery
    {
        IQueryDefinition Build(TQuery query);
    }
}