using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using common;
using Newtonsoft.Json;
using common.Caching;
using logic;
using logic.Model;

namespace msite.JSDK
{
    /// <summary>
    /// 通过config接口注入权限验证配置
    /// </summary>
    public partial class RegConfig : PageBaseHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string errorInfo;
            JsSDKConfigModel sdkConfig = this.GetSignPackage(out errorInfo);
            LogHelper.Write(JsonConvert.SerializeObject(sdkConfig));
            if (sdkConfig != null)
            {
                Response.Write(string.Format("wx.config({0});", JsonConvert.SerializeObject(sdkConfig)));
            }
            else
            {
                Response.Write(string.Format("/*出错：{0}*/", errorInfo));
            }
        }

        public int CurrentCustomerID
        {
            get
            {
                return this.GetQueryString("customerid", 0);
            }
        }


        /// <summary>
        /// 获得要生成注入配置的实体
        /// </summary>
        /// <param name="errorInfo">出错信息</param>
        /// <returns></returns>
        private JsSDKConfigModel GetSignPackage(out string errorInfo)
        {
            errorInfo = "";
            try
            {
                string appId = this.AppId(this.CurrentCustomerID);
                string jsapi_ticket = GetTicket(this.CurrentCustomerID, appId, this.AppSecret(this.CurrentCustomerID));
                string url = this.CallUrl;
                string timestamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
                string noncestr = StringHelper.CreateNoncestr();

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("jsapi_ticket", jsapi_ticket);
                parameters.Add("noncestr", noncestr);
                parameters.Add("timestamp", timestamp);
                parameters.Add("url", url);

                string bizString = StringHelper.FormatBizQueryParaMap(parameters, false, false);

                string signature = StringHelper.Sha1(bizString);

                return new JsSDKConfigModel()
                {
                    appId = appId,
                    debug = this.DebugMode,
                    jsApiList = this.JsApiList,
                    nonceStr = noncestr,
                    signature = signature,
                    timestamp = timestamp
                };
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                LogHelper.Write(ex.Message + "|" + ex.StackTrace);
                return null;
            }
        }

        public string GetTicket(int customerId, string appid, string appsecret)
        {
            string accessToken = "", ticket;
            WxAccessTokenTicket model = UserService.Instance.GetToken(customerId);
            if (model != null)
            {
                if (model.gettime.AddMinutes(90) > DateTime.Now)
                {
                    accessToken = model.value;
                    model = UserService.Instance.GetTicket(customerId);
                    if (model != null)
                    {
                        if (model.gettime.AddMinutes(90) > DateTime.Now)
                            return model.value;
                    }
                    else
                    {
                        ticket = BuildTicket(accessToken);
                        UserService.Instance.updateTicket(customerId, ticket);
                        return ticket;
                    }
                }
            }

            accessToken = BuildAccessToken(appid, appsecret, customerId);
            ticket = BuildTicket(accessToken);
            UserService.Instance.UpdateToken(customerId, accessToken);
            UserService.Instance.updateTicket(customerId, ticket);

            return ticket;
        }
        /// <summary>
        /// 保存微信ticket
        /// </summary>
        /// <param name="customerId">商户Id</param>
        /// <param name="openId"></param>
        public void SetTicket(int customerId, string openId)
        {
            //CookieHelper.SetCookieValByCurrentDomain(this.GetOpenIdDataKey(customerId), 1, EncryptHelper.Encrypt(openId, ENCRYPTKEY));

            ////双保险，session也存储
            //HttpContext.Current.Session[this.GetOpenIdDataKey(customerId)] = openId;
        }


        public string BuildAccessToken(string appid, string appsecret, int customerId)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, appsecret);
            IHttpForm _httpForm = new HttpForm("", 15000, true, 8);
            HttpFormResponse _response = _httpForm.Get(new HttpFormGetRequest()
            {
                Url = url
            });
            Dictionary<object, object> dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(_response.Response);
            if (dict.ContainsKey("access_token"))
            {
                return dict["access_token"].ToString();
            }
            throw new Exception(string.Format("非Senparc获取AccessToken失败->appid:{0},appsecret:{1}，商户Id:{2}", appid, appsecret, customerId));
        }

        private string BuildTicket(string accessToken)
        {
            try
            {
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?type=jsapi&access_token={0}", accessToken);
                IHttpForm _httpForm = new HttpForm("", 1500, true, 8);
                HttpFormResponse _response = _httpForm.Get(new HttpFormGetRequest()
                {
                    Url = url
                });
                Dictionary<object, object> dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(_response.Response);
                if (dict.ContainsKey("ticket"))
                {
                    return dict["ticket"].ToString();
                }
                LogHelper.Write("GetTicket(获取jsapi_ticket失败):" + _response.Response);
                return "";
            }
            catch (Exception ex)
            {
                LogHelper.Write("WxJsApiTicketProvider->GetTicket(string accessToken)报错：" + ex.Message + "|accessToken：" + accessToken);
                return "";
            }
        }


        /// <summary>
        /// 要调用的接口列表
        /// </summary>
        public List<string> JsApiList
        {
            get
            {
                string apilist = this.GetQueryString("apilist", "checkJsApi,onMenuShareTimeline,onMenuShareAppMessage,onMenuShareQQ,onMenuShareWeibo");
                return new List<string>(apilist.Split(','));

                //return new List<string>(new string[]{
                //                    "checkJsApi",
                //                    "onMenuShareTimeline",
                //                    "onMenuShareAppMessage",
                //                    "onMenuShareQQ",
                //                    "onMenuShareWeibo",
                //                    "hideMenuItems",
                //                    "showMenuItems",
                //                    "hideAllNonBaseMenuItem",
                //                    "showAllNonBaseMenuItem",
                //                    "translateVoice",
                //                    "startRecord",
                //                    "stopRecord",
                //                    "onRecordEnd",
                //                    "playVoice",
                //                    "pauseVoice",
                //                    "stopVoice",
                //                    "uploadVoice",
                //                    "downloadVoice",
                //                    "chooseImage",
                //                    "previewImage",
                //                    "uploadImage",
                //                    "downloadImage",
                //                    "getNetworkType",
                //                    "openLocation",
                //                    "getLocation",
                //                    "hideOptionMenu",
                //                    "showOptionMenu",
                //                    "closeWindow",
                //                    "scanQRCode",
                //                    "chooseWXPay",
                //                    "openProductSpecificView",
                //                    "addCard",
                //                    "chooseCard",
                //                    "openCard"
                //                            });
            }
        }

        /// <summary>
        /// 调试模式
        /// </summary>
        public bool DebugMode
        {
            get
            {
                return this.GetQueryString("debug", 0) == 1;
            }
        }

        /// <summary>
        /// 调用接口的当前网址
        /// </summary>
        public string CallUrl
        {
            get
            {
                string url = Server.UrlDecode(this.GetQueryString("url", ""));
                if (url == "" && Request.UrlReferrer != null)
                {
                    url = Request.UrlReferrer.ToString();
                }
                return url;
            }
        }

    }

    public class JsSDKConfigModel
    {
        public bool debug { get; set; }
        public string appId { get; set; }
        public string timestamp { get; set; }
        public string nonceStr { get; set; }
        public string signature { get; set; }
        public List<string> jsApiList { get; set; }
    }
}