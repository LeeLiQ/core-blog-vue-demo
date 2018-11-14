namespace blog_webapi_vue.AOP
{
    public interface ICaching
    {
        object Get(string cacheKey);
        void Set(string cachekey, object cachValue);
    }
}