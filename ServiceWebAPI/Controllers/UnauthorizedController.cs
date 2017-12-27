using System;
using System.Web.Http;

namespace ServiceWebAPI.Controllers
{
    /// <summary>
    /// Unauthorized
    /// </summary>
    public class UnauthorizedController : ApiController
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns>respose</returns>
        [HttpPost]
        public IHttpActionResult Login([FromBody]Reqesut request)
        {
            var a = this.Request.Headers;
            var b = this.RequestContext;
            var h = System.Web.HttpContext.Current.Request.Headers;
            var reqStr = Encrypt.DecryptAes(request.Data, Encrypt.DefaultKey);
            if (string.IsNullOrWhiteSpace(reqStr))
            {
                Result result = new Result { Message = "密钥错误" };
                return Ok(result);
            }
            var loginRequest = reqStr.DeserializeObject<LoginRequest>();
            if (loginRequest == null)
            {
                Result result = new Result { Message = "登录参数错误" };
                return Ok(result);
            }
            Result<string> loginResult = LoginDb(loginRequest);
            return Ok(loginResult);
        }

        /// <summary>
        /// 链接数据库登录
        /// </summary>
        /// <param name="reqesut">请求</param>
        /// <returns>结果</returns>
        private Result<string> LoginDb(LoginRequest request)
        {
            Result<string> result = new Result<string>();
            if (request.UserName == "admin")
            {
                var loginResult = new LoginResult
                {
                    AccessToken = Encrypt.GetNewKey(),
                    Encryptionkey = Encrypt.GetNewKey(),
                    TokenExpiration = DateTime.Now.AddYears(100),
                    UserInfo = new UserInfo
                    {
                        Email = "5545555441415@163.com",
                        Id = 585544,
                        RegTime = DateTime.Now,
                        UserName = "admin"
                    }
                };

                UserManager.AddTokenToTokenCache(loginResult);
                var data = Encrypt.EncryptAes(loginResult.SerializeObject(), Encrypt.DefaultKey);
                result.IsSuccess = true;
                result.Data = data;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "登录错误";
            }
            return result;
        }
    }
}