using System;

namespace ServiceWebAPI
{
    /// <summary>
    /// 登录结果
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// AccessToken
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime TokenExpiration { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string Encryptionkey { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get; set; }
    }
}