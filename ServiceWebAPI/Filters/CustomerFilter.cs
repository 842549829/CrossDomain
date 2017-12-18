using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ServiceWebAPI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CustomerFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Reqesut request = (Reqesut)actionContext.ActionArguments["request"];
            var retModel = CheckToken(request, actionContext);
            if (retModel.IsSuccess)
            {
                request.Data = Encrypt.DecryptAes(request.Data, UserManager.LoginTokenDataList[request.AccessToken].Encryptionkey);
                request.NewAccessToken = retModel.Message;
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Accepted, retModel);
            }
        }

        public Result CheckToken(Reqesut requestModel, HttpActionContext actionContext)
        {
            Result result = new Result();
            if (requestModel == null || string.IsNullOrWhiteSpace(requestModel.Data))
            {
                result.IsSuccess = false;
                result.Message = "请求主体为空";
                return result;
            }
            if (string.IsNullOrWhiteSpace(requestModel.AccessToken))
            {
                result.IsSuccess = false;
                result.Message = "密钥为空";
                return result;
            }
            if (!UserManager.LoginTokenDataList.ContainsKey(requestModel.AccessToken))
            {
                result.IsSuccess = false;
                result.Message = "密钥已过期";
                return result;
            }

            //var loginResult = UserManager.LoginTokenDataList[requestModel.AccessToken];
            //var newLoginResult = new LoginResult
            //{
            //    AccessToken = Encrypt.GetNewKey(),
            //    Encryptionkey = Encrypt.GetNewKey(),
            //    TokenExpiration = DateTime.Now.AddYears(100),
            //    UserInfo = loginResult.UserInfo,
            //};
            //UserManager.AddTokenToTokenCache(newLoginResult);
            //result.Message = newLoginResult.AccessToken;
            result.IsSuccess = true;
            return result;
        }
    }
}