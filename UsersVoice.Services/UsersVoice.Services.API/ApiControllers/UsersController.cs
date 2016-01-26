using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MediatR;
using UsersVoice.Services.API.CQRS.Queries;
using UsersVoice.Services.API.CQRS.Queries.Models;
using UsersVoice.Services.Infrastructure.Common;
using UsersVoice.Services.Infrastructure.Web;

namespace UsersVoice.Services.API.ApiControllers
{
    /// <summary>
    /// Users API endpoint
    /// </summary>
    [RoutePrefix("api/users")]
    public class UsersController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            if (mediator == null) throw new ArgumentNullException("mediator");
            _mediator = mediator;
        }

        /// <summary>
        /// users archive endpoint
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(PagedCollection<UserArchiveItem>))]
        public async Task<IHttpActionResult> Get([FromUri] UsersArchiveQuery filter)
        {
            filter = filter ?? new UsersArchiveQuery();
            var items = await _mediator.SendAsync(filter);
            return OkOrNotFound(items);
        }

        /// <summary>
        /// user details endpoint
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}")]
        [ResponseType(typeof(UserDetails))]
        public async Task<IHttpActionResult> GetById(Guid id)
        {
            var filter = new UserDetailsQuery(id);
            var item = await _mediator.SendAsync(filter);
            return OkOrNotFound(item);
        }
    }
}
