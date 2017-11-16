using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace ServiceWebAPI
{
    [CustomerFilter]
    public class HomeController : ApiController
    {

        public Reqesut Decrypt(Reqesut request)
        {
            if (request != null)
            {
                var authorization = this.Request.Headers.Authorization?.Scheme;
                var opneid = GetOpnedId();
                var key = Get(authorization + opneid);
                if (string.IsNullOrWhiteSpace(authorization) || string.IsNullOrWhiteSpace(key))
                {
                    key = "ABCDEFGABCDEFG12ABCDEFGABCDEFG12";
                }
                request.Data = Encrypt.DecryptAes(request.Data, key);
                var newKey = GetNewKey();
                request.AccessToken = newKey;
                Insert(newKey + opneid, newKey);
                return request;
            }
            return null;
        }

        public IHttpActionResult LoginSys(Reqesut request)
        {
            // 1.解密
            Decrypt(request);
            // 2.反序列化
            // 3.执行业务
            // 5.加密返回数据
            Result<string> ret = new Result<string>
            {
                IsSuccess = true,
                Message = "OK",
                Data = "OK"
            };

            return Ok(ret);
        }

        [HttpPost]
        public IHttpActionResult ListDataTest([FromBody]Reqesut request)
        {
            Result<string> res = new Result<string>();
            try
            {
                var requestmodel = request.Data.DeserializeObject<object>();
                var data = new { T = "12",
                    KK = "xx",
                    AccessToken = request.AccessToken,
                    Encryptionkey = UserManager.LoginTokenDataList[request.AccessToken].Encryptionkey
                };
                res.IsSuccess = true;
                res.Data = data.ToJson(request.AccessToken);
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
            }
            return Ok(res);
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