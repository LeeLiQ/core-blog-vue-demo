using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace blog_webapi_vue.AuthHelper
{
    public class JwtTokenAuth
    {
        private readonly RequestDelegate _next;

        public JwtTokenAuth(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
                return _next(httpContext);

            var tokenHeader = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            TokenModelJWT tm = JwtHelper.SerializeJWT(tokenHeader);

            var claimList = new List<Claim>();
            var claim = new Claim(ClaimTypes.Role, tm.Role);
            claimList.Add(claim);

            var identity = new ClaimsIdentity(claimList);
            var principal = new ClaimsPrincipal(identity);

            httpContext.User = principal;
            return _next(httpContext);
        }
    }
}