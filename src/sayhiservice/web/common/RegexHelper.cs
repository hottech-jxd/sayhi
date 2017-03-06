using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace common
{
    public class RegexHelper
    {
        const string _RegNoNagInt = @"^\d+$";　//非负整数（正整数 + 0） 
        const string _RegRectInt = @"^[0-9]*[1-9][0-9]*$";　　//正整数 
        const string _RegNoRectInt = @"^((-\d+)(0+))$";　　//非正整数（负整数 + 0） 
        const string _RegNagInt = @"^-[0-9]*[1-9][0-9]*$";　　//负整数 
        const string _RegInt = @"^-?\d+$";　　//整数 
        const string _RegNoNagDouble = @"^\d+(\.\d+)?$";　　//非负浮点数（正浮点数 + 0） 
        const string _RegRectDouble = @"^(([0-9]+\.[0-9]*[1-9][0-9]*)([0-9]*[1-9][0-9]*\.[0-9]+)([0-9]*[1-9][0-9]*))$";　　//正浮点数 
        const string _RegNoRectDouble = @"^((-\d+(\.\d+)?)(0+(\.0+)?))$";　　//非正浮点数（负浮点数 + 0） 
        const string _RegNagDouble = @"^(-(([0-9]+\.[0-9]*[1-9][0-9]*)([0-9]*[1-9][0-9]*\.[0-9]+)([0-9]*[1-9][0-9]*)))$";　　//负浮点数 
        const string _RegDouble = @"^(-?\d+)(\.\d+)?$";　　//浮点数 
        const string _RegABC = @"^[A-Za-z]+$";　　//由26个英文字母组成的字符串 
        const string _RegabcBig = @"^[A-Z]+$";　　//由26个英文字母的大写组成的字符串 
        const string _Regabc = @"^[a-z]+$";　　//由26个英文字母的小写组成的字符串 
        const string _RegABCNum = @"^[A-Za-z0-9]+$";　　//由数字和26个英文字母组成的字符串 
        const string _RegNomalInput = @"^\w+$";　　//由数字、26个英文字母或者下划线组成的字符串 
        const string _RegABCNumCh = @"^[a-zA-Z0-9\u4e00-\u9fa5]+$";//字母数字汉字
        const string _RegABCCh = @"^[a-zA-Z\u4e00-\u9fa5]+$";//字母数字汉字
        const string _RegCh = @"^[\u4e00-\u9fa5]+$";//汉字
        const string _RegNum = @"^[0-9]+$";//数字
        /// <summary>
        /// email地址
        /// </summary>
        const string _RegEmail = @"^[.\-_a-zA-Z0-9]+@([.\-_a-zA-Z0-9]+\.)+[a-zA-Z0-9]{2,3}$";
        /// <summary>
        /// 小数
        /// </summary>
        const string _RegDecimal = @"^[0].\d{1,2}|[1]$";
        /// <summary>
        /// 电话号码
        /// </summary>
        const string _RegTel = @"^(\d+-)?(\d{4}-?\d{7}|\d{3}-?\d{8}|^\d{7,8})(-\d+)?$";
        const string _RegFixedTel = @"^\d{3}-\d{8}|\d{4}-\d{7}$";
        const string _RegMobileNo = @"^(1)\d{10}$";
        /// <summary>
        /// 真实手机号码
        /// </summary>
        const string _RegRealMobileNo = @"^[1]+[3,4,5,7,8,9]+\d{9}";
        /// <summary>
        /// 年月日
        /// </summary>
        //const string _RegDate = @"^2\d{3}-(?:0?[1-9]|1[0-2])-(?:0?[1-9]|[1-2]\d|3[0-1])(?:0?[1-9]|1\d|2[0-3]):(?:0?[1-9]|[1-5]\d):(?:0?[1-9]|[1-5]\d)$";
        /// <summary>
        /// 后缀名
        /// </summary>
        const string _RegPostfix = @"^\.(?i:gif|jpg)$";
        /// <summary>
        /// Ip
        /// </summary>
        const string _RegIP = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";

        const string _RegChinese = @"^[\u4e00-\u9fa5]$";
        const string _RegDoubleByte = @"^[^\x00-\xff]$";
        const string _RegCard = @"^\d{15}|\d{18}$";
        const string _RegPost = @"^[1-9]\d{5}(?!\d)$";
        const string _RegQQ = @"^[1-9][0-9]{4,}$";

        const string _RegGUID = @"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-f]{4}-[0-9a-fA-f]{4}-[0-9a-fA-F]{12}$";

        const string _RegChapNo = @"^[0-9]{8}_[0-9]{6}$";

        const string _RegChapTime = @"^[0-9]{2}:[0-9]{2}:[0-9]{2}$";
        const string _RegLoginName = @"^[A-Za-z]+[A-Za-z0-9_\-]+$";

        const string _RegVersion = @"^[0-9].[0-9].[0-9]$";
        public RegexHelper() { }

        /// <summary>
        /// 由英文字母组成
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidABC(string input)
        {
            return Regex.IsMatch(input, _RegABC);
        }

        /// <summary>
        /// 由数字组成
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidInt(string input)
        {
            return Regex.IsMatch(input, _RegInt);
        }

        /// <summary>
        /// 由数字和26个英文字母组成的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidABCNum(string input)
        {
            return Regex.IsMatch(input, _RegABCNum);
        }
        /// <summary>
        /// 由数字、26个英文字母或者下划线组成的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidNomalInput(string input)
        {
            return Regex.IsMatch(input, _RegNomalInput);
        }
        /// <summary>
        /// 验证Email地址 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string input)
        {
            // Return true if input is in valid e-mail format. 
            return Regex.IsMatch(input, _RegEmail);
        }
        /// <summary>
        /// 验证是否为小数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidDecimal(string input)
        {
            return Regex.IsMatch(input, _RegDecimal);
        }
        /// <summary>
        /// 验证是否为电话号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidTel(string input)
        {
            return Regex.IsMatch(input, _RegTel);
        }

        /// <summary>
        /// 固定电话
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidFixedTel(string input)
        {
            return Regex.IsMatch(input, _RegFixedTel);
        }

        /// <summary>
        /// 手机号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidMobileNo(string input)
        {
            return Regex.IsMatch(input, _RegMobileNo);
        }
        /// <summary>
        /// 验证真实的手机号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidRealMobileNo(string input)
        {
            return Regex.IsMatch(input, _RegRealMobileNo);
        }

        /// <summary>
        /// 验证年月日
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //public static bool IsValidDate(string input)
        //{
        //    return Regex.IsMatch(input, _RegDate);
        //}
        /// <summary>
        /// 验证后缀名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidPostfix(string input)
        {
            return Regex.IsMatch(input, _RegPostfix);
        }
        /// <summary>
        /// 验证IP
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidIp(string input)
        {
            return Regex.IsMatch(input, _RegIP);
        }

        /// <summary>
        /// 中文字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidChinese(string input)
        {
            return Regex.IsMatch(input, _RegChinese);
        }
        /// <summary>
        /// 双字节字符(包括汉字在内)：
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidDoubleByte(string input)
        {
            return Regex.IsMatch(input, _RegDoubleByte);
        }
        /// <summary>
        /// 身份证[中国的身份证为15位或18]
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidCard(string input)
        {
            return Regex.IsMatch(input, _RegCard);
        }
        /// <summary>
        ///中国邮政编码为6位数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidPost(string input)
        {
            return Regex.IsMatch(input, _RegPost);
        }
        /// <summary>
        ///腾讯QQ号从10000开始
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidQQ(string input)
        {
            return Regex.IsMatch(input, _RegQQ);
        }

        /// <summary>
        /// 字母数字和汉字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidAbcNumCh(string input)
        {
            return Regex.IsMatch(input, _RegABCNumCh);
        }

        /// <summary>
        /// 字母汉字组合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidAbcCh(string input)
        {
            return Regex.IsMatch(input, _RegABCCh);
        }

        /// <summary>
        /// 汉字组成
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidCh(string input)
        {
            return Regex.IsMatch(input, _RegCh);
        }

        /// <summary>
        /// 有数字组成
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidNum(string input)
        {
            return Regex.IsMatch(input, _RegNum);
        }

        /// <summary>
        /// 是否是统一编码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsGUID(string input)
        {
            return Regex.IsMatch(input, _RegGUID);
        }

        /// <summary>
        /// 是否是课件编号如 20100809_120945
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChapNo(string input)
        {
            return Regex.IsMatch(input, _RegChapNo);
        }

        /// <summary>
        /// 是否是课件时长格式如 01:02:03
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChapTime(string input)
        {
            return Regex.IsMatch(input, _RegChapTime);
        }

        /// <summary>
        /// 用户名，登录名(字母数字_-)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsLoginName(string input)
        {
            return Regex.IsMatch(input, _RegLoginName);
        }

        /// <summary>
        /// 版本号 如1.10.6
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsVersion(string input)
        {
            return Regex.IsMatch(input, _RegVersion);
        }
    }
}
