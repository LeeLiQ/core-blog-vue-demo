using System;

namespace blog_webapi_vue.Common.Attributes
{
    /// <summary>
    /// Decorate methods that need cache with this attribute. It works as a filter
    /// only to methods need caching.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public sealed class CachingAttribute : Attribute
    {
        public int AbsoluteExpiration { get; set; } = 30;
    }
}