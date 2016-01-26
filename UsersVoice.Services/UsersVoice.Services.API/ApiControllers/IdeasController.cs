using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MediatR;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.API.CQRS.Queries;
using UsersVoice.Services.API.CQRS.Queries.Models;
using UsersVoice.Services.Infrastructure.Common;
using UsersVoice.Services.Infrastructure.Web;

namespace UsersVoice.Services.API.ApiControllers
{
    [RoutePrefix("api/ideas")]
    public class IdeasController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public IdeasController(IMediator mediator)
        {
            if (mediator == null) throw new ArgumentNullException("mediator");
            _mediator = mediator;
        }

        [HttpGet]
        [ResponseType(typeof(PagedCollection<IdeaArchiveItem>))]
        public async Task<IHttpActionResult> Get([FromUri]IdeasArchiveQuery filter)
        {
            filter = filter ?? new IdeasArchiveQuery();
            var items = await _mediator.SendAsync(filter);
            return OkOrNotFound(items);
        }

        [HttpGet, Route("{id}")]
        [ResponseType(typeof(IdeaDetails))]
        public async Task<IHttpActionResult> GetById(Guid id)
        {
            var filter = new IdeaDetailsQuery(id);
            var item = await _mediator.SendAsync(filter);
            return OkOrNotFound(item);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]CreateIdea command)
        {
            await _mediator.PublishAsync(command);
            return Ok();
        }

        [HttpPost, Route("{id}/cancel")]
        public async Task<IHttpActionResult> PostSetCancelStatus([FromBody]CancelIdea command)
        {
            await _mediator.PublishAsync(command);
            return Ok();
        }

        [HttpPost, Route("{id}/approve")]
        public async Task<IHttpActionResult> PostSetApprovedStatus([FromBody]ApproveIdea command)
        {
            await _mediator.PublishAsync(command);
            return Ok();
        }

        [HttpPost, Route("{id}/implement")]
        public async Task<IHttpActionResult> PostSetImplementedStatus([FromBody]ImplementIdea command)
        {
            await _mediator.PublishAsync(command);
            return Ok();
        }
    }
}
