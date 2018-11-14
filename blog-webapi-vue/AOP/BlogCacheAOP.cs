using System;
using System.Linq;
using Castle.DynamicProxy;

namespace blog_webapi_vue.AOP
{
    public class BlogCacheAOP : IInterceptor
    {
        private ICaching _cache;
        public BlogCacheAOP(ICaching cache)
        {
            _cache = cache;
        }
        public void Intercept(IInvocation invocation)
        {
            var cacheKey = CustomCacheKey(invocation);
            var cacheValue = _cache.Get(cacheKey);
            if (cacheValue != null)
            {
                invocation.ReturnValue = cacheValue;
                return;
            }

            invocation.Proceed();

            if (!string.IsNullOrWhiteSpace(cacheKey))
                _cache.Set(cacheKey, invocation.ReturnValue);
        }

        private string CustomCacheKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            var methodArguments = invocation.Arguments.Select(GetArgumentValue).Take(3).ToList(); // get first three param list

            string key = $"{typeName}:{methodName}:";
            foreach (var param in methodArguments)
            {
                key += $"{param}:";
            }

            return key.TrimEnd(':');
        }

        private string GetArgumentValue(object arg)
        {
            if (arg is int || arg is long || arg is string)
                return arg.ToString();

            if (arg is DateTime)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            return "";
        }
    }
}