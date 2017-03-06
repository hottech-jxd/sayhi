using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace common
{
    public class CookieHelper
    {
        private CookieHelper() { }


        #region 设置Cookie值
        /// <summary>
        /// T对象序列化后写入cookie
        /// </summary>
        /// <param name="cookieName">Cookie名</param>
        /// <param name="targetObject">需要序列化的对象,必须标记了可被序列化</param>
        /// <param name="iExpires">COOKIE对象有效时间（分钟），1表示永久有效，0和负数都表示不设有效时间，大于等于2表示具体有效分钟，525600分=1年=(60*24*365)</param>
        public static void SetCookieVal<T>(string cookieName, T targetObject, int iExpires)
        {
            SetCookieVal(cookieName, iExpires, SerializeHelper.BinarySerializeObjectToBase64String<T>(targetObject));
        }
        /// <summary>
        /// 反序列化cookie的值返回user对象
        /// </summary>
        /// <param name="cookieName">Cookie名</param>
        /// <returns></returns>
        public static T GetCookieVal<T>(string cookieName)
        {
            return SerializeHelper.BinaryDeserializeBase64StringToObject<T>(GetCookieVal(cookieName));
        }

        /// <summary>
        /// 设置Cookie值，不使用共享主域名的方式
        /// </summary>
        /// <param name="cookieName">Cookie名</param>
        /// <param name="iExpires">COOKIE对象有效时间（分钟），1表示永久有效，0和负数都表示不设有效时间，大于等于2表示具体有效分钟，525600分=1年=(60*24*365)</param>
        /// <param name="cookieValue">Cookie值</param>
        public static void SetCookieValByCurrentDomain(string cookieName, int iExpires, string cookieValue)
        {
            WriteCookies(cookieName, iExpires, cookieValue, false);
        }

        /// <summary>
        /// 设置Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名</param>
        /// <param name="iExpires">COOKIE对象有效时间（分钟），1表示永久有效，0和负数都表示不设有效时间，大于等于2表示具体有效分钟，525600分=1年=(60*24*365)</param>
        /// <param name="cookieValue">Cookie值</param>
        public static void SetCookieVal(string cookieName, int iExpires, string cookieValue, bool shareDomain = true)
        {
            WriteCookies(cookieName, iExpires, cookieValue, shareDomain);
        }
        /// <summary>
        /// 设置Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名</param>
        /// <param name="iExpires">COOKIE对象有效时间（分钟），1表示永久有效，0和负数都表示不设有效时间，大于等于2表示具体有效分钟，525600分=1年=(60*24*365)</param>
        /// <param name="keyAndValue ">Cookie键名和值</param>
        public static void SetCookieVal(string cookieName, int iExpires, NameValueCollection keyAndValue)
        {
            WriteCookies(cookieName, iExpires, keyAndValue);
        }
        #endregion

        #region 获取Cookie值
        /// <summary>
        /// 获取Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名</param>
        /// <returns></returns>
        public static string GetCookieVal(string cookieName)
        {
            return GetCookies(cookieName);
        }
        /// <summary>
        /// 获取Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名</param>
        /// <param name="keyName">键名</param>
        /// <returns></returns>
        public static string GetCookieVal(string cookieName, string keyName)
        {
            return GetCookies(cookieName, keyName);
        }

        /// <summary>
        /// 获取Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名</param>
        /// <param name="keyName">键名数组</param>
        /// <returns></returns>
        public static string GetCookieVal(string cookieName, string[] keyName)
        {

            StringBuilder strVal = new StringBuilder();
            for (int i = 0; i < keyName.Length; i++)
            {
                strVal.AppendLine(keyName[i] + "：" + GetCookies(cookieName, keyName[i]));
            }
            return strVal.ToString();
        }
        #endregion

        #region 删除Cookie值
        /// <summary>
        /// 删除Cookie值，当前域的cookie
        /// </summary>
        /// <param name="cookieName">Cookie名</param>
        public static void DelCookieValByCurrentDomain(string cookieName)
        {
            WriteCookies(cookieName, -1, "", false);
        }

        /// <summary>
        /// 删除Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名</param>
        public static void DelCookieVal(string cookieName)
        {
            WriteCookies(cookieName, -1, "", true);
            WriteCookies(cookieName, -1, "", false);

            //DelCookie(cookieName);
        }
        #endregion

        #region 判断Cookies是否存在
        public static bool CookieExist(string CookieName)
        {
            return HttpContext.Current.Request.Cookies[CookieName] != null;
        }
        #endregion

        public static HttpCookieCollection GetCookies()
        {
            return HttpContext.Current.Request.Cookies;
        }


        #region 删除Cookie方法
        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="CookiesName">Cookie名</param>
        private static void DelCookie(string CookiesName)
        {
            HttpContext.Current.Request.Cookies.Remove(CookiesName);
            if (HttpContext.Current.Request.Cookies[CookiesName] != null)
            {
                HttpCookie cookie = new HttpCookie(CookiesName.Trim());
                cookie.Value = "";
            }
        }
        #endregion

        #region 获取Cookie值
        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="CookiesName">Cookie名</param>
        /// <returns></returns>
        private static string GetCookies(string CookiesName)
        {
            if (HttpContext.Current.Request.Cookies[CookiesName] == null)
            {
                return null;
            }

            return HttpContext.Current.Request.Cookies[CookiesName].Value;
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="CookiesName">Cookie名</param>
        /// <param name="KeyName">键值</param>
        /// <returns></returns>
        private static string GetCookies(string CookiesName, string KeyName)
        {
            if (HttpContext.Current.Request.Cookies[CookiesName] == null)
            {
                return null;
            }
            else
            {
                string str = HttpContext.Current.Request.Cookies[CookiesName].Value;
                string str2 = KeyName + "=";
                if ((0 == 0) && (str.IndexOf(str2) == -1))
                {
                    return null;
                }
                return HttpContext.Current.Request.Cookies[CookiesName][KeyName];
            }
        }
        #endregion

        #region 设置Cookie方法
        /// <summary>
        /// 设置Cookie多值
        /// </summary>
        /// <param name="CookiesName">Cookie名称</param>
        /// <param name="IExpires">COOKIE对象有效时间（分钟），1表示永久有效，0和负数都表示不设有效时间，大于等于2表示具体有效分钟，525600分=1年=(60*24*365)</param>
        /// <param name="CookiesKeyValueCollection">键/值</param>
        private static void WriteCookies(string CookiesName, int IExpires, NameValueCollection CookiesKeyValueCollection)
        {
            string[] strArray;
            int num;
            HttpCookie cookie = new HttpCookie(CookiesName.Trim());
            goto Label_00BF;
        Label_0032:
            HttpContext.Current.Response.Cookies.Add(cookie);
            return;
        Label_0049:
            if (IExpires != 1)
            {
                cookie.Expires = DateTime.Now.AddSeconds((double)IExpires);
                if ((((uint)num) + ((uint)IExpires)) < 0)
                {
                    goto Label_00CA;
                }
            }
            else
            {
                cookie.Expires = DateTime.MaxValue;
            }
            goto Label_0032;
        Label_0097:
            if (num < strArray.Length)
            {
                goto Label_00CA;
            }
            if (8 != 0)
            {
                if (IExpires > 0)
                {
                    goto Label_0049;
                }
                goto Label_0032;
            }
        Label_00BF:
            strArray = CookiesKeyValueCollection.AllKeys;
            num = 0;
            goto Label_0097;
        Label_00CA:
            do
            {
                string str = strArray[num];
                cookie[str] = CookiesKeyValueCollection[str].Trim();
                num++;
            }
            while ((((uint)num) | 0xff) == 0);
            goto Label_0097;
        }

        /// <summary>
        /// 设置Cookie值(写入主域名)
        /// </summary>
        /// <param name="CookiesName">Cookie名</param>
        /// <param name="IExpires">COOKIE对象有效时间（分钟），1表示永久有效，负数表示立即失效，0都表示不设有效时间，大于等于2表示具体有效分钟，525600分=1年=(60*24*365)</param>
        /// <param name="CookiesValue">Cookie值</param>
        private static void WriteCookies(string CookiesName, int IExpires, string CookiesValue, bool shareDomain)
        {
            HttpCookie cookie = new HttpCookie(CookiesName.Trim());
            cookie.Value = CookiesValue.Trim();

            if (shareDomain)
            {
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                string regex = @"(?i)http://(\w+\.){2,3}(com(\.cn)?|cn|net)\b";
                if (Regex.Match(url, regex, RegexOptions.IgnoreCase).Success)
                {
                    string strReg = @"http://[^\.]*\.(?<domain>[^/]*)";
                    Match m = Regex.Match(url, strReg, RegexOptions.IgnoreCase);
                    if (m.Success)
                    {

                        cookie.Domain = m.Groups["domain"].Value;
                    }
                }
            }
            if (IExpires == 1)
            {
                cookie.Expires = DateTime.MaxValue;
            }
            else if (IExpires >= 2)
            {
                cookie.Expires = DateTime.Now.AddMinutes((double)IExpires);
            }
            else if (IExpires < 0)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        #endregion

        /// <summary>
        /// 清除所有cookie --郭孟稳
        /// </summary>
        /// <param name="share">是否删除同一域名下的所有cookie，默认只清空当前网站cookie</param>
        public static void ClearAllCookie(bool share = false)
        {
            /**********************获取当前域名*******************************/
            string domain = string.Empty;
            if (share)
            {
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                string regex = @"(?i)http://(\w+\.){2,3}(com(\.cn)?|cn|net)\b";
                if (Regex.Match(url, regex, RegexOptions.IgnoreCase).Success)
                {
                    string strReg = @"http://[^\.]*\.(?<domain>[^/]*)";
                    Match m = Regex.Match(url, strReg, RegexOptions.IgnoreCase);
                    if (m.Success)
                    {
                        domain = m.Groups["domain"].Value;
                    }
                }
            }
            int limit = HttpContext.Current.Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                string cookieName = HttpContext.Current.Request.Cookies[i].Name;
                HttpCookie aCookie = new HttpCookie(cookieName, null);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                if (share)
                {
                    aCookie.Domain = domain;
                    HttpContext.Current.Response.Cookies.Add(aCookie);
                }
                else
                {
                    HttpContext.Current.Response.Cookies.Add(aCookie);
                }
            }
        }
    }
}
