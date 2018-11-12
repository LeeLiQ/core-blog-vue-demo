using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using blog_webapi_vue.IRepository.BASE;
using blog_webapi_vue.Model;

namespace blog_webapi_vue.IRepository
{
    public interface IAdvertisementRepository:IBaseRepository<Advertisement>
    {
        int Sum(int a, int b);

        // int Add(Advertisement model);
        // bool Delete(Advertisement model);
        // bool Update(Advertisement model);
        // List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression);
    }
}