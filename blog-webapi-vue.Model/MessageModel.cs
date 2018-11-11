using System.Collections.Generic;

namespace blog_webapi_vue.Model
{
    /// <summary>
    /// Generic message model for end to end data delivery
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageModel<T>
    {
        /// <summary>
        /// Success or not
        /// </summary>
        /// <value>boolean</value>
        public bool Success { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        /// <value>string</value>
        public string Msg { get; set; }

        /// <summary>
        /// Returned value
        /// </summary>
        /// <value>List of type T</value>
        public List<T> Data { get; set; }
    }
}