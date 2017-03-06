using common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;

namespace admin_modeng
{
    /// <summary>
    /// 结果架构
    /// </summary>
    [Serializable]
    public class ResultModel
    {

        public ResultModel() { }

        public ResultModel(BaseService.ApiStatusCode statusCode)
        {
            this.code = (int)statusCode;
            this.desc = statusCode.ToString();
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public int code { get; set; }

        private string _desc = "";
        /// <summary>
        /// 接口描述
        /// </summary>
        public string desc
        {
            get { return _desc; }
            set { _desc = value; }
        }
        private object _result = new object();
        /// <summary>
        /// 对象
        /// </summary>
        public object result
        {
            get { return _result; }
            set { _result = value; }
        }

        private int _pageIndex = 1;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int index
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }
        /// <summary>
        ///设置返回结果
        /// </summary>
        /// <param name="statusCode">返回状态</param>
        public void setResult(BaseService.ApiStatusCode statusCode)
        {
            this.code = (int)statusCode;
            this.desc = statusCode.ToString();
        }
        /// <summary>
        /// 设置返回结果
        /// </summary>
        /// <param name="statusCode">返回状态</param>
        /// <param name="data">返回内容</param>
        public void setResult(BaseService.ApiStatusCode statusCode, object data)
        {
            this.code = (int)statusCode;
            this.desc = statusCode.ToString();
            this.result = data;
        }
        /// <summary>
        /// 设置返回结果
        /// </summary>
        /// <param name="statusCode">返回状态</param>
        /// <param name="data">返回内容</param>
        /// <param name="PageIndex">当前页码</param>
        public void setResult(BaseService.ApiStatusCode statusCode, object data, int PageIndex)
        {
            this.code = (int)statusCode;
            this.desc = statusCode.ToString();
            this.result = data;
            this.index = PageIndex;
        }
    }

    [WebService]
    public class BaseService : System.Web.Services.WebService
    {

        public ResultModel result = null;
        public int recordCount = 0;
        public int pageSize = 20;
        public readonly IsoDateTimeConverter timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };

        public BaseService()
        {
            Context.Response.ContentType = "application/json";
            if (result == null)
                result = new ResultModel(ApiStatusCode.OK);
        }

        public JObject prams { get; set; }

        /// <summary>
        /// 验证签名
        /// </summary>
        public void VerifySignCheck()
        {
            try
            {
                prams = GetParams(Context.Request);
                if (enableSign)
                {

                    string signParam = prams.Value<string>("sign");//签名
                    long timestamp = prams.Value<long>("timestamp");
                    if (prams == null || prams.Count <= 0)
                    {
                        result.setResult(ApiStatusCode.签名无效, "签名无效");
                    }
                    else
                    {
                        DateTime date = DateTime.Now;
                        if (timestamp < GetUTCTime(date.AddMinutes(-5)) || timestamp > GetUTCTime(date.AddMinutes(5)))
                        {
                            result.setResult(ApiStatusCode.签名无效, "签名无效");
                        }
                        StringBuilder sbParam = new StringBuilder();
                        Dictionary<string, string> parameters = new Dictionary<string, string>();
                        foreach (var item in prams)
                        {
                            parameters.Add(item.Key.ToLower(), item.Value.ToString());
                        }
                        string sign = HotSignatureHelper.BuildSign(parameters, AppSecrect, new HotSignatureHelper.BuildSettingModel()
                        {
                            JoinFormat = HotSignatureHelper.PreSignStrJoinFormatOptions.UrlParameter,
                            EcryptType = HotSignatureHelper.EncryptTypeOptions.MD5_UTF8_32,
                            SaltPosition = HotSignatureHelper.SaltAppendPositionOptions.Suffix
                        });
                        if (string.IsNullOrEmpty(signParam) || !signParam.Equals(sign))
                        {
                            result.setResult(ApiStatusCode.签名无效, "签名无效");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Debug(string.Format("StackTrace:{0},Message:{1}", ex.StackTrace, ex.Message));
                result.setResult(ApiStatusCode.服务器错误, ex.Message);
            }
            if (result.code != (int)ApiStatusCode.OK)
                JsonResult(result);
        }

        /// <summary>
        /// 获取 get/post 数据
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        private JObject GetParams(HttpRequest Request)
        {
            JObject p = new JObject();

            if (Request.HttpMethod.ToLower() == "post") //post 数据请求
            {
                NameValueCollection values =Request.Form;// HttpContext.Current.
                foreach (string m in values.Keys)
                {
                    p.Add(m, HttpUtility.UrlDecode(values[m]));//20160106
                }
            }
            else  //get 数据请求
            {
                string url = Request.Url.ToString();
                if (url == null || url == "")
                    return p;
                int questionMarkIndex = url.IndexOf('?');
                if (questionMarkIndex == url.Length - 1)
                    return p;
                string ps = url.Substring(questionMarkIndex + 1);
                if (ps != null && !string.IsNullOrEmpty(ps))
                {
                    // 开始分析参数对   
                    Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
                    MatchCollection mc = re.Matches(ps);
                    foreach (Match m in mc)
                    {
                        p.Add(m.Result("$2"), HttpUtility.UrlDecode(m.Result("$3")));
                    }
                }
            }
            return p;
        }


        /// <summary>
        /// JSON输出
        /// </summary>
        /// <param name="content"></param>
        public void JsonResult(object result)
        {
            var resultStr = JsonConvert.SerializeObject(result, timeConverter);
            Context.Response.Write(resultStr);
            Context.Response.End();
        }

        /// <summary>
        /// 是否开启debug模式
        /// </summary>
        public bool DebugMode
        {
            get
            {
                try
                {
                    string debug = ConfigHelper.GetConfigString("debugMode", "false");
                    if (!string.IsNullOrEmpty(debug))
                        return Convert.ToBoolean(debug);
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 是否开启签名
        /// </summary>
        public bool enableSign
        {
            get
            {
                try
                {
                    string enableSign = ConfigHelper.GetConfigString("enableSign", "false");
                    if (!string.IsNullOrEmpty(enableSign))
                        return Convert.ToBoolean(enableSign);
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// 签名密钥
        /// </summary>
        private string AppSecrect
        {
            get
            {
                try
                {
                    string _AppSecrect = ConfigHelper.GetConfigString("AppSecrect", "");
                    if (!string.IsNullOrEmpty(_AppSecrect))
                        return _AppSecrect;
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 获取UTC时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private Int64 GetUTCTime(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (Int64)(time - startTime).TotalMilliseconds;
        }


        /// <summary>
        /// 接口状态
        /// </summary>
        public enum ApiStatusCode
        {
            OK = 200,
            参数错误 = 300,
            失败 = 302,
            签名无效 = 402,
            地址错误 = 404,
            服务器错误 = 500,
        }
    }
}