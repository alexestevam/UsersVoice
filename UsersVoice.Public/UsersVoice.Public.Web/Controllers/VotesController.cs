using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UsersVoice.Public.Web.Models;

namespace UsersVoice.Public.Web.Controllers
{
    public class VotesController : BaseApiController
    {
        public async Task<HttpResponseMessage> Post(Vote model)
        {
#if DEBUG
            await Database.VoteIdea(model);
            return Request.CreateResponse(HttpStatusCode.OK);
#else
            //using (var client = new HttpClient())
            //{
            //    var url = GetUrl("/users");

            //    var response = await client.GetAsync(url);

            //    var content = await response.Content.ReadAsStringAsync();
            //    dynamic users = JsonConvert.DeserializeObject<dynamic>(content);

            //    if (users == null || users.Items == null || users.Items.Count == 0)
            //        return;

            //    model.VoterId = users.Items.First.Id; 
            //    var responsePost = await client.PostAsJsonAsync(GetUrl("/vote"), model);
            //    string x = await responsePost.Content.ReadAsStringAsync();
            //}
            return null;
#endif
        } 

    }
}