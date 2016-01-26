using System.Threading.Tasks;

namespace UsersVoice.Services.Common.CQRS.Queries
{
    public interface IQueryRunner<TResult>
    {
        Task<TResult> RunAsync(IQueryDefinition filter);
    }
}