using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace ServiceWebAPI
{
    public class HomeController : ApiController
    {
        /// <summary>
        /// 获取用户信息集合的方法
        /// </summary>
        /// <returns>返回用户信息集合</returns>
        public IHttpActionResult ListDataTest(Ret ret)
        {
            if (ret.IsSuccess)
            {
                var token = GetCookie("Token");
                if (string.IsNullOrWhiteSpace(token))
                {
                    WriteCookie("Token", ret.Data, DateTime.Now.AddDays(1));
                }
            }
            var head = this.Request;
            var authorization = head.Headers.Authorization;
          
            List<UserInfo> list = new List<UserInfo>()
               {
                new UserInfo()
                {
                 Id = 1,
                 UserName = "张三",
                 UserPass = "FDASDFAS",
                 Email = "zhangsan@163.com",
                 RegTime = DateTime.Now
                },
                new UserInfo()
                {
                 Id = 2,
                 UserName = "李四",
                 UserPass = "FDASDFAS",
                 Email = "lisi@163.com",
                 RegTime = DateTime.Now
                },
                new UserInfo()
                {
                 Id = 3,
                 UserName = "王五",
                 UserPass = "FDASDFAS",
                 Email = "wangwu@163.com",
                 RegTime = DateTime.Now
                },
                new UserInfo()
                {
                 Id = 4,
                 UserName = "赵六",
                 UserPass = "FDASDFAS",
                 Email = "zhaoliu@163.com",
                 RegTime = DateTime.Now
                },
                new UserInfo()
                {
                 Id = 5,
                 UserName = "田七",
                 UserPass = "FDASDFAS",
                 Email = "tianqi@163.com",
                 RegTime = DateTime.Now
                },
                new UserInfo()
                {
                 Id = 6,
                 UserName = "王八",
                 UserPass = "FDASDFAS",
                 Email = "wangba@163.com",
                 RegTime = DateTime.Now
                }
               };
            return Ok(list);
        }

        /// <summary>
        /// 写Cookie
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="dateTime">有效时间</param>
        public static void WriteCookie(string key, string value, DateTime dateTime)
        {
            string domain = HttpContext.Current.Request.Url.Host;
            if (!HttpContext.Current.Request.Url.IsLoopback)
            {
                domain = domain.Substring(domain.IndexOf(".", StringComparison.Ordinal) + 1);
            }
            HttpCookie systemCookie = HttpContext.Current.Response.Cookies[key];
            if (systemCookie != null)
            {
                HttpContext.Current.Response.Cookies.Remove(key);
            }
            systemCookie = new HttpCookie(key, value)
            {
                Expires = dateTime,

                Path = "/"
            };
            if (!Regex.IsMatch(domain, @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$"))
            {
                systemCookie.Domain = domain;
            }
            HttpContext.Current.Response.Cookies.Add(systemCookie);
        }

        /// <summary>
        /// 根据cookie名称，获取cookie值
        /// </summary>
        /// <param name="key">cookie名称</param>
        /// <returns>Cookie</returns>
        public static string GetCookie(string key)
        {
            if (HttpContext.Current == null)
            {
                return string.Empty;
            }
            if (HttpContext.Current.Request == null)
            {
                return string.Empty;
            }
            if (HttpContext.Current.Request.Cookies == null)
            {
                return string.Empty;
            }
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                return cookie.Value;
            }
            return string.Empty;
        }
    }
}