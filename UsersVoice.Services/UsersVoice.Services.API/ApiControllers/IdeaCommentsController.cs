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
    [RoutePrefix("api/ideaComments")]
    public class IdeaCommentsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public IdeaCommentsController(IMediator mediator)
        {
            if (mediator == null) throw new ArgumentNullException("mediator");

            _mediator = mediator;
        }

        [HttpGet]
        [ResponseType(typeof(PagedCollection<IdeaCommentArchiveItem>))]
        public async Task<IHttpActionResult> Get([FromUri]IdeaCommentsArchiveQuery filter)
        {
            filter = filter ?? new IdeaCommentsArchiveQuery();
            var items = await _mediator.SendAsync(filter);

            return OkOrNotFound(items);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]CreateIdeaComment command)
        {
            await _mediator.PublishAsync(command);
            return Ok();
        }

    }
}
