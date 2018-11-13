using System.Collections.Generic;
using System.Threading.Tasks;
using blog_webapi_vue.IServices.BASE;
using blog_webapi_vue.Model;

namespace blog_webapi_vue.IServices
{
    public interface IBlogArticleServices : IBaseServices<BlogArticle>
    {
        Task<List<BlogArticle>> GetBlogs();
    }
}