using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using UsersVoice.Public.Web.Models;

namespace UsersVoice.Public.Web.Controllers
{
    public class BaseApiController : ApiController
    {
        private readonly static string BaseUrl = "http://40.85.103.249/api";

        static BaseApiController()
        {
            BaseUrl = ConfigurationManager.AppSettings["DataServiceUrl"];
        }

        public string GetUrl(string url)
        {
            return string.Format("{0}{1}", BaseUrl, url);
        }

        protected async Task<IHttpActionResult> ServeCollection<TModel>(string url)
        {
            var items = await FetchData<PagedCollection<TModel>>(url);
            return Ok(items.Items);
           
        }

        protected async Task<IHttpActionResult> ServeDetails<TModel>(string url)
        {
            var item = await FetchData<TModel>(url);
            return Ok(item);
        }

        protected async Task<TModel> FetchData<TModel>(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(GetUrl(url));

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                    return default(TModel);
                var item = JsonConvert.DeserializeObject<TModel>(content);
                return item;
            }
        }
    }
}