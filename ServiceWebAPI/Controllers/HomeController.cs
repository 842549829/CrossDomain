using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace ServiceWebAPI
{
    public class HomeController : ApiController
    {
        public IHttpActionResult ListDataTest(Reqesut request)
        {
            var authorization = this.Request.Headers.Authorization.Scheme;
            var opneid =   GetOpnedId();
            var key = Get(authorization + opneid);
            if (string.IsNullOrWhiteSpace(key))
            {
                key = "ABCDEFGABCDEFG12ABCDEFGABCDEFG12";
            }
            var data = Encrypt.DecryptAes(request.Data, key);
            var value = GetNewKey();
            Insert(value + opneid, value);
            Result<string> ret = new Result<string>
            {
                IsSuccess = true,
                Message = "清楚参数为空",
                Authorization = value,
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