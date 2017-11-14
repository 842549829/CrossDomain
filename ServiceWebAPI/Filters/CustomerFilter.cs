using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ServiceWebAPI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CustomerFilter : ActionFilterAttribute
    {
        /// <summary>
        /// token缓存信息 key token,value 对象信息 可以更改写到redis
        /// </summary>
        public static Dictionary<string, Reqesut> loginTokenDataList = new Dictionary<string, Reqesut>();

        public void AddTokenToTokenCache(Reqesut retmodel, HttpActionContext actionContext)
        {
            var key = actionContext.Request.Headers.Authorization.Scheme;

            if (loginTokenDataList.ContainsKey(key) == false)
            {
                loginTokenDataList.Add(key, retmodel);
            }
            else
            {
                loginTokenDataList[key] = retmodel;
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Reqesut reqesut = (Reqesut)actionContext.ActionArguments["model"];
            var retModel = CheckToken(reqesut, actionContext);
            retModel.Data = Encrypt.DecryptAes(reqesut.Data, loginTokenDataList[retModel.AccessToken].AccessToken);
        }

        private void ShowMessage<T>(HttpActionContext actionContext, T message)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Accepted, message);
        }

        
        public  Reqesut CheckToken(Reqesut requestModel, HttpActionContext actionContext)
        {
            string accessToken = actionContext.Request.Headers.Authorization.Scheme;
            AddTokenToTokenCache(new Reqesut
            {
                AccessToken = accessToken,
            }, actionContext);
            return requestModel;
        }
    }
}