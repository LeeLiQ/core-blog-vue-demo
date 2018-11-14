using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using blog_webapi_vue.IRepository;
using blog_webapi_vue.IServices;
using blog_webapi_vue.Model;
using blog_webapi_vue.Repository;
using blog_webapi_vue.Services.BASE;

namespace blog_webapi_vue.Services
{
    public class AdvertisementServices : BaseServices<Advertisement>, IAdvertisementServices
    {
        // private readonly IAdvertisementRepository _dal;
        public AdvertisementServices(IAdvertisementRepository dal) : base(dal)
        {
            // _dal = dal;
            // base.baseDal = dal;  // There is something not so clean here.
        }
        // private readonly IAdvertisementRepository _repo = new AdvertisementRepository();

        // public int Add(Advertisement model)
        // {
        //     return _repo.Add(model);
        // }

        // public bool Delete(Advertisement model)
        // {
        //     return _repo.Delete(model);
        // }

        // public List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression)
        // {
        //     return _repo.Query(whereExpression);
        // }

        // public int Sum(int a, int b)
        // {
        //     return _repo.Sum(a, b);
        // }

        // public bool Update(Advertisement model)
        // {
        //     return _repo.Update(model);
        // }

        public int Sum(int i, int j)
        {
            return i + j;
        }
    }
}