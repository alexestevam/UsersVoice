using System;
using System.Threading.Tasks;
using System.Web.Http;
using UsersVoice.Public.Web.Models;

namespace UsersVoice.Public.Web.Controllers
{
    [RoutePrefix("api/Areas")]
    public class AreasController : BaseApiController
    {
        public async Task<IHttpActionResult> Get()
        {
#if DEBUG
            var areas = await Database.GetAreasAsync();
            return Ok(areas);
#else
            var url = "/areas?pageSize=10&page=0";

            return await ServeCollection<Area>(url);
#endif
        }

        [HttpGet, Route("{areaID}/ideas")]
        public async Task<IHttpActionResult> Get([FromUri]IdeaFilter filter)
        {
#if DEBUG
            var ideas = await Database.GetIdeasAsync(filter.AreaId);
            return Ok(ideas);
#else

            var url = string.Format("/ideas?sortBy={0}&sortDirection={1}&page={2}&pageSize={3}",
               filter.SortBy, filter.SortDirection, filter.Page, filter.PageSize);
            if (filter.AreaId != Guid.Empty)
                url += "&areaId=" + filter.AreaId;

            return await ServeCollection<Idea>(url);
#endif
        }

    }
}