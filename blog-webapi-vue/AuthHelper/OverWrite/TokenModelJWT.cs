namespace blog_webapi_vue.AuthHelper
{
    /// <summary>
    /// token model
    /// </summary>
    public class TokenModelJWT
    {
        /// <summary>
        /// user id
        /// </summary>
        /// <value>long</value>
        public long Uid { get; set; }

        /// <summary>
        /// user role
        /// </summary>
        /// <value>string</value>
        public string Role { get; set; }

        /// <summary>
        /// user duty
        /// </summary>
        /// <value>string</value>
        public string Work { get; set; }
    }
}