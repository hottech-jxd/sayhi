using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace common
{
    public static class GlobalProvider
    {
        #region 判断字符串是否为空,返回true为空，否则不为空
        /// <summary>
        /// 判断字符串是否为空,返回true为空，否则不为空
        /// </summary>
        /// <param name="boolValue">字符串值</param>
        /// <returns></returns>
        public static bool StrIsNull(this string boolValue)
        {
            if (boolValue != null && boolValue != "" && boolValue.ToLower() != "null" && !string.IsNullOrEmpty(boolValue) && boolValue.ToString().Trim().Length != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}
