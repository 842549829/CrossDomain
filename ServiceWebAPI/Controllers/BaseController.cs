using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ServiceWebAPI
{
    public class BaseController : ApiController
    {
        protected LoginResult GetNewKey(Reqesut request)
        {
            var oldLoginResult = UserManager.LoginTokenDataList[request.AccessToken];
            var loginResult = new LoginResult
            {
                AccessToken = Encrypt.GetNewKey(),
                Encryptionkey = Encrypt.GetNewKey(),
                TokenExpiration = DateTime.Now.AddYears(100),
                UserInfo = new UserInfo
                {
                    Email = oldLoginResult.UserInfo.Email,
                    Id = oldLoginResult.UserInfo.Id,
                    RegTime = oldLoginResult.UserInfo.RegTime,
                    UserName = oldLoginResult.UserInfo.UserName,
                }
            };
            UserManager.AddTokenToTokenCache(loginResult);
            return loginResult;
        }

        private void Insert(string key, string value)
        {
            HttpRuntime.Cache.Insert(key, value);
        }

        private string Get(string key)
        {
            var value = HttpRuntime.Cache.Get(key);
            if (value != null)
            {
                return value.ToString();
            }
            return null;
        }

        private string GetOpnedId()
        {
            var token = Util.GetCookie("Token1");
            if (string.IsNullOrWhiteSpace(token))
            {
                string opnedId = GetNewKey();
                Util.WriteCookie("Token1", opnedId, DateTime.Now.AddYears(1));
                return opnedId;
            }
            return token;
        }

        private string GetNewKey()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty).ToUpper();
        }
    }
}