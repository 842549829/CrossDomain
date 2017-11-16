namespace ServiceWebAPI
{
    /// <summary>
    /// 登录请求信息
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// 帐号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
    }
}