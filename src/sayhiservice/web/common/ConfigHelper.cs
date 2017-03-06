using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace common
{
    public class ConfigHelper
    {
        /// <summary>
        /// 默认链接，key=MssqlDBConnectionString
        /// </summary>
        public static string MssqlDBConnectionString { get { return GetConfigString("MssqlDBConnectionString"); } }

        #region 方法
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetConfigString(string key, string defaultValue)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key] ?? defaultValue;
        }
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigString(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key] ?? "";
        }
     
        #endregion

    }
}
