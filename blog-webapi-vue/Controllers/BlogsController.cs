using System.Collections.Generic;
using System.Threading.Tasks;
using blog_webapi_vue.IServices;
using blog_webapi_vue.Model;
using blog_webapi_vue.Services;
using Microsoft.AspNetCore.Mvc;
namespace blog_webapi_vue.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BlogsController : Controller
    {
        private readonly IAdvertisementServices _advertisementServices;

        public BlogsController(IAdvertisementServices advertisementServices)
        {
            _advertisementServices = advertisementServices;
        }

        [HttpGet]
        public int Get(int i, int j)
        {
            // IAdvertisementServices advertisementServices = new AdvertisementServices();
            return _advertisementServices.Sum(i, j);
        }

        // GET: api/Blog/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<List<Advertisement>> Get(int id)
        {
            // IAdvertisementServices advertisementServices = new AdvertisementServices();
            return await _advertisementServices.Query(d => d.Id == id);
        }

        // POST: api/Blog
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Blog/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}