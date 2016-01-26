using System;
using System.Threading.Tasks;
using System.Web.Http;
using UsersVoice.Public.Web.Models;

namespace UsersVoice.Public.Web.Controllers
{
    [RoutePrefix("api/IdeaComments")]
    public class IdeaCommentsController : BaseApiController
    {
        public async Task<IHttpActionResult> Get([FromUri] Guid ideaId)
        {
#if DEBUG
            var comments = await Database.GetCommentsAsync(ideaId);
            return Ok(comments);
#else
            var url = "/ideaComments?ideaId=" + ideaId;

            return await ServeCollection<Comment>(url);
#endif
        }

    }
}