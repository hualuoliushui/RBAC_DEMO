using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace FormsAuth_Demo2.Models
{
    public class Forms
    {
        public IIdentity _identity;

        public Forms(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException("ticket");
            }
            _identity = new FormsIdentity(ticket);
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        /// <summary>
        /// 执行登录前的验证
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        public static bool Verigy(string loginName,string password){
            return true;
        }
        /// <summary>
        /// 执行用户登录操作
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="expiration">登录Cookie的过期时间，单位：分钟。</param>
        public static void Login(string loginName,int expiration){
            if (string.IsNullOrEmpty(loginName))
                throw new ArgumentNullException("loginName");

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
               2, loginName, DateTime.Now, DateTime.Now.AddDays(1), true, "");

            string cookieValue = FormsAuthentication.Encrypt(ticket);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                cookieValue);
            cookie.HttpOnly = true;
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Domain = FormsAuthentication.CookieDomain;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (expiration > 0)
                cookie.Expires = DateTime.Now.AddMinutes(expiration);

            HttpContext context = HttpContext.Current;
            if (context == null)
                throw new InvalidOperationException();

            context.Response.Cookies.Remove(cookie.Name);
            context.Response.Cookies.Add(cookie);
        }
    }
}