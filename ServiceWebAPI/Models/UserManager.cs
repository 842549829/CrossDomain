using System.Collections.Generic;

namespace ServiceWebAPI
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserManager
    {
        /// <summary>
        /// token缓存信息 key token,value 对象信息
        /// </summary>
        public static Dictionary<string, LoginResult> LoginTokenDataList = new Dictionary<string, LoginResult>();

        /// <summary>
        /// Adds the token to token cache.
        /// </summary>
        /// <param name="retmodel">The retmodel.</param>
        public static void AddTokenToTokenCache(LoginResult retmodel)
        {
            // 该地方可以考虑存入到 redis 设置有效期为30分钟
            if (!LoginTokenDataList.ContainsKey(retmodel.AccessToken))
            {
                LoginTokenDataList.Add(retmodel.AccessToken, retmodel);
            }
            else
            {
                LoginTokenDataList[retmodel.AccessToken] = retmodel;
            }
        }
    }
}