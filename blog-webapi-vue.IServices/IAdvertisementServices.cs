using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using blog_webapi_vue.IServices.BASE;
using blog_webapi_vue.Model;

namespace blog_webapi_vue.IServices
{
    public interface IAdvertisementServices:IBaseServices<Advertisement>
    {
        int Sum(int a, int b);
        // int Add(Advertisement model);
        // bool Delete(Advertisement model);
        // bool Update(Advertisement model);
        // List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression);
    }
}