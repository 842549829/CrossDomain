using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace ServiceWebAPI
{
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
                Authorization = request.AccessToken,
                Data = "OK"
            };
            
            return Ok(ret);
        }

        public IHttpActionResult ListDataTest(Reqesut request)
        {
            Decrypt(request);

            Result<string> ret = new Result<string>
            {
                IsSuccess = true,
                Message = "清楚参数为空",
                Authorization = request.AccessToken,
                Data = "xxxxx"
            };
            return Ok(ret);
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