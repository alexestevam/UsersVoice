using System.Web.Http;

namespace UsersVoice.Services.Infrastructure.Web
{
    public abstract class ApiControllerBase : ApiController
    {
        protected IHttpActionResult OkOrNotFound<T>(T data)
        {
            if (null == data) return this.NotFound();
            return this.Ok(data);
        }
    }
}
