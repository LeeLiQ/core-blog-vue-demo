using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using blog_webapi_vue.Common.Attributes;
using blog_webapi_vue.Common.Helpers;
using blog_webapi_vue.Common.Redis;
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
        private readonly IBlogArticleServices _blogArticleServices;
        private readonly IRedisCacheManager _redisCacheManager;

        public BlogsController(IAdvertisementServices advertisementServices, IBlogArticleServices blogArticleServices, IRedisCacheManager redisCacheManager)
        {
            _redisCacheManager = redisCacheManager;
            _advertisementServices = advertisementServices;
            _blogArticleServices = blogArticleServices;
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


        //Demo for AOP
        [HttpGet]
        [Route("GetBlogs")]
        public async Task<List<BlogArticle>> GetBlogs()
        {
            // var connect = Appsettings.app(new string[] { "AppSettings", "RedisCaching", "ConnectionString" });//按照层级的顺序，依次写出来

            List<BlogArticle> blogArticleList = new List<BlogArticle>();

            if (_redisCacheManager.Get<object>("Redis.Blog") != null)
            {
                blogArticleList = _redisCacheManager.Get<List<BlogArticle>>("Redis.Blog");
            }
            else
            {
                blogArticleList = await _blogArticleServices.Query(d => d.bID > 5);
                _redisCacheManager.Set("Redis.Blog", blogArticleList, TimeSpan.FromHours(2));//缓存2小时
            }

            return blogArticleList;
        }
    }
}