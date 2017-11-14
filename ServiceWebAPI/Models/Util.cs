using System;
using System.Text.RegularExpressions;
using System.Web;

namespace ServiceWebAPI
{
    public class Util
    {
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