using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace ServiceWebAPI
{
    [CustomerFilter]
    public class HomeController : BaseController
    {
        [HttpPost]
        public IHttpActionResult ListDataTest([FromBody]Reqesut request)
        {
            Result<string> res = new Result<string>();
            try
            {
                LoginResult loginResult = GetNewKey(request);
                var requestmodel = request.Data.DeserializeObject<object>();
                var data = new
                {
                    T = "12",
                    KK = "xx",
                    AccessToken = loginResult.AccessToken,
                    Encryptionkey = loginResult.Encryptionkey
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
    }
}