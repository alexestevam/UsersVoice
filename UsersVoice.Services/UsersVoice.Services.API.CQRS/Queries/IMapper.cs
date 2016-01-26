using System.Threading.Tasks;

namespace UsersVoice.Services.API.CQRS.Queries
{
    public interface IMapper<in TEntity, TModel>
    {
        Task<TModel> MapAsync(TEntity entity);
    }
}