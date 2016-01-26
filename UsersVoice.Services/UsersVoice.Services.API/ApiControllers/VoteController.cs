using System;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using UsersVoice.Services.API.CQRS.Commands;
using UsersVoice.Services.API.CQRS.Queries;
using UsersVoice.Services.Infrastructure.Web;

namespace UsersVoice.Services.API.ApiControllers
{
    [RoutePrefix("api/vote")]
    public class VoteController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public VoteController(IMediator mediator)
        {
            if (mediator == null) throw new ArgumentNullException("mediator");
            _mediator = mediator;
        }

        [HttpGet, Route("hasVoted")]
        public async Task<IHttpActionResult> GetHasVoted(Guid ideaId, Guid userId)
        {
            var query = new HasVotedQuery(userId, ideaId);
            var result = await _mediator.SendAsync(query);
            return OkOrNotFound(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] VoteIdea command)
        {
            await _mediator.PublishAsync(command);
            return Ok();
        }

        [HttpPost, Route("unvote")]
        public async Task<IHttpActionResult> Post([FromBody] UnvoteIdea command)
        {
            await _mediator.PublishAsync(command);
            return Ok();
        }
    }
}
