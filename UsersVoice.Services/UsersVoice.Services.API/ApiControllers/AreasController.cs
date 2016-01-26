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
    [RoutePrefix("api/areas")]
    public class AreasController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        
        public AreasController(IMediator mediator)
        {
            if (mediator == null) throw new ArgumentNullException("mediator");

            _mediator = mediator;
        }

        [HttpGet]
        [ResponseType(typeof(PagedCollection<AreaArchiveItem>))]
        public async Task<IHttpActionResult> Get([FromUri]AreasArchiveQuery filter)
        {
            filter = filter ?? new AreasArchiveQuery();
            var items = await _mediator.SendAsync(filter);

            return OkOrNotFound(items);
        }

        [HttpGet, Route("{id}")]
        [ResponseType(typeof(AreaDetails))]
        public async Task<IHttpActionResult> GetById(Guid id)
        {
            var filter = new AreaDetailsQuery(id);
            var item = await _mediator.SendAsync(filter);
            return OkOrNotFound(item);
        }
    }
}
