using blog_webapi_vue.Services.BASE;
using blog_webapi_vue.Model;
using blog_webapi_vue.IServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using blog_webapi_vue.IRepository;

namespace blog_webapi_vue.Services
{
    public class BlogArticleServices : BaseServices<BlogArticle>, IBlogArticleServices
    {
        IBlogArticleRepository dal;
        public BlogArticleServices(IBlogArticleRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<BlogArticle>> GetBlogs()
        {
            var bloglist = await dal.Query(a => a.bID > 0, a => a.bID);

            return bloglist;

        }
    }
}