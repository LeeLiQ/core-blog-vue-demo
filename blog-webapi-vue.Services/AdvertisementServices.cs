using blog_webapi_vue.IRepository;
using blog_webapi_vue.IServices;
using blog_webapi_vue.Repository;

namespace blog_webapi_vue.Services
{
    public class AdvertisementServices : IAdvertisementServices
    {
        private readonly IAdvertisementRepository _repo = new AdvertisementRepository();
        public int Sum(int a, int b)
        {
            return _repo.Sum(a, b);
        }
    }
}