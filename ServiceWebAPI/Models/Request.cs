using System;

namespace ServiceWebAPI
{
    public class Reqesut
    {
        /// <summary>
        /// 事先获取的（双方约定是否可空）
        /// </summary>
        /// <value>The access token.</value>
        public string AccessToken { get; set; }

        /// <summary>
        /// 源IP
        /// </summary>
        /// <value>The opera system.</value>
        public string NewAccessToken { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }
    }
}