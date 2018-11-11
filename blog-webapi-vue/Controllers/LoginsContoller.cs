using blog_webapi_vue.AuthHelper;

using Microsoft.AspNetCore.Mvc;

namespace blog_webapi_vue.Controllers
{
    [Produces("application/json")]
    [Route("api/login")]
    public class LoginsContoller : ControllerBase
    {
        [HttpGet]
        [Route("Token2")]
        public JsonResult GetJWTStr(long id = 1, string sub = "Admin")
        {
            var tokenModel = new TokenModelJWT();
            tokenModel.Uid = id;
            tokenModel.Role = sub;

            var jwtStr = JwtHelper.IssueJWT(tokenModel);
            return new JsonResult(jwtStr);
        }
    }
}