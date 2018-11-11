using blog_webapi_vue.IRepository;

namespace blog_webapi_vue.Repository
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        public int Sum(int a, int b)
        {
            return a + b;
        }
    }
}