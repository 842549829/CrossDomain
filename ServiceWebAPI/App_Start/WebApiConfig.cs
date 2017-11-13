using System.Web.Http;
using System.Web.Http.Cors;

namespace ServiceWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            EnableCrossSiteRequests(config);
        }

        /// <summary>
          /// 允许跨域调用
          /// </summary>
          /// <param name="config">config</param>
        private static void EnableCrossSiteRequests(HttpConfiguration config)
        {
            //对所有的请求来源没有任何限制
            var cors = new EnableCorsAttribute(origins: "*", headers: "*", methods: "*")
            {
                SupportsCredentials = true
            };
            config.EnableCors(cors);
        }
    }
}