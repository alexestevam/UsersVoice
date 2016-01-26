using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using UsersVoice.Public.Web.Models;

namespace UsersVoice.Public.Web.Controllers
{

    [RoutePrefix("api/Ideas")]
    public class IdeasController : BaseApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> Get([FromUri]IdeaFilter filter)
        {
            filter = filter ?? new IdeaFilter();

#if DEBUG
          
            var ideas = await Database.GetLatestIdeasAsync(filter.PageSize);
            return Ok(ideas);
#else
            var url = string.Format("/ideas?sortBy={0}&sortDirection={1}&page={2}&pageSize={3}",
              filter.SortBy, filter.SortDirection, filter.Page, filter.PageSize);
            if (filter.AreaId != Guid.Empty)
                url += "&areaId=" + filter.AreaId;

            return await ServeCollection<Idea>(url);
#endif
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> GetById(Guid id)
        {
#if DEBUG

            var idea = await Database.GetIdeaAsync(id);
            return Ok(idea);
#else

            var url = "/ideas/"+id;
            var item = await FetchData<Idea>(url);
            if (null != item)
            {
                url = "/ideaComments?ideaId=" + id;
                var comments = await FetchData<PagedCollection<Comment>>(url);
                item.Comments = (null != comments) ? comments.Items : Enumerable.Empty<Comment>();
            }
            return Ok(item);
#endif
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(Idea idea)
        {
#if DEBUG
            await Database.AddIdea(idea);
#else
            var result = string.Empty;

            using (var client = new HttpClient())
            {
                var data = new CreateIdeaCommand(Guid.NewGuid(), idea.AreaId, idea.AuthorId, idea.Title, idea.Description);
                var responsePost = await client.PostAsJsonAsync(GetUrl("/ideas"), data);

                result = await responsePost.Content.ReadAsStringAsync();
                if (!responsePost.IsSuccessStatusCode)
                    return InternalServerError();
            }
#endif

            return Ok();
        }

        [HttpPost, Route("{id}/comment")]
        public async Task<IHttpActionResult> PostComment(Comment comment)
        {
            comment.CommentId = Guid.NewGuid();
#if DEBUG
            await Database.SaveCommentAsync(comment);
            return Ok();
#else
            var result = string.Empty;
            using (var client = new HttpClient())
            {
                var responsePost = await client.PostAsJsonAsync(GetUrl("/ideaComments"), comment);
                result = await responsePost.Content.ReadAsStringAsync();
                if (!responsePost.IsSuccessStatusCode)
                    return InternalServerError();
            }
            return Ok(result);
#endif
        }

        [HttpGet, Route("{ideaId}/votes/{userId}")]
        public async Task<IHttpActionResult> HasVoted(Guid ideaId, Guid userId)
        {
#if DEBUG
            var result = await Database.HasVoted(ideaId, userId);
            return Ok(result);
#else
            var url = String.Format("/vote/hasVoted?ideaId={0}&userId={1}", ideaId, userId);

            return await ServeDetails<bool>(url);
#endif

        }

        [HttpPost, Route("vote/")]
        public async Task<IHttpActionResult> Vote(Vote newVote)
        {
           
#if DEBUG
            var user = await Database.AddVote(newVote);
            return Ok(user);
#else
            var url = "/vote/";
            if (newVote.Points < 1)
                url += "unvote/";

            using (var client = new HttpClient())
            {
                var responsePost = await client.PostAsJsonAsync(GetUrl(url), newVote);
                var result = await responsePost.Content.ReadAsStringAsync();
                if (!responsePost.IsSuccessStatusCode)
                    return InternalServerError(new Exception(result));
            }

            url = "/users/" + newVote.VoterId;
            var user = await FetchData<User>(url);
            if (null == user)
                return NotFound();
            return Ok(user);
#endif
        }
    }
}