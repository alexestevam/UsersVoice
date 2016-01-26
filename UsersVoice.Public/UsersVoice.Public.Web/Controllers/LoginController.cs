using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using UsersVoice.Public.Web.Models;

namespace UsersVoice.Public.Web.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : BaseApiController
    {
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> GetUserByUsername(LoginRequestModel model)
        {

#if DEBUG

            var user = await Database.GetUserByEmailAsync(model.userName);
            return Ok(user);

#else

            var url = "/users?email=" + model.userName;
            var items = await FetchData<PagedCollection<User>>(url);
            if (null == items || 0 == items.TotalItemsCount)
                return NotFound();
            return Ok(items.Items.FirstOrDefault());
#endif

        }
    }
}
