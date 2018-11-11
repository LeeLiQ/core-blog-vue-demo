using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using blog_webapi_vue.Model;

using Microsoft.IdentityModel.Tokens;

namespace blog_webapi_vue.AuthHelper
{
    public class JwtHelper
    {
        public static string secretKey { get; set; } = "sdfsdfsrty45634kkhllghtdgdfss345t678fs";

        /// <summary>
        /// Issue JWT string
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJWT(TokenModelJWT tokenModel)
        {
            var claims = new Claim[]
            {
                // default Claims
                new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),

                // expiry time. Caution: JWT has its own cache expiry time.
                new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddSeconds(100)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Iss, "Blog.Core"),
                new Claim(JwtRegisteredClaimNames.Aud, "wr"),

                //Microsoft UseAuthentication Role.
                new Claim(ClaimTypes.Role, tokenModel.Role)
            };

            //secret
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtHelper.secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: "Blog.Core",
                claims : claims,
                signingCredentials : creds
            );

            var jwtHanlder = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHanlder.WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// resolve token string
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModelJWT SerializeJWT(string jwtStr)
        {
            var jwtHanlder = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHanlder.ReadJwtToken(jwtStr);

            object role = new object();
            try
            {
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                throw;
            }

            var tokenModel = new TokenModelJWT
            {
                Uid = (jwtToken.Id).ObjToInt(),
                Role = role != null?role.ObjToString() : string.Empty
            };
            return tokenModel;
        }
    }
}