using common;
using logic;
using logic.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace msite
{
    public class BaseClass : PageBaseHelper
    {
        public void LoadInit(int customerid, bool tryreg = true)
        {            
            this.CurrentCustomerID = customerid;
            //判断来源微信环境
            bool isWechatEnvironment = this.ValidateMicroMessenger();
            if (!isWechatEnvironment)
            {
                Response.Redirect("/WechatPropmt.html");
            }
            string callbakUrl = Server.UrlEncode(HttpContext.Current.Request.RawUrl);

            string oauthUrl = "/OAuth2/WeixinAuthorize.aspx?userinfo=0&scope=1&uda=0&customerid=" + this.CurrentCustomerID + "&tryreg=" + (tryreg ? 1 : 0) + "&redirecturl=" + callbakUrl;

            //用户授权
            WeixinOAuthUserInfoModel model =UserService.Instance.GetUserInfo(this.CurrentCustomerID);
            if (model != null)
            {
                this.OpenId = model.Openid;
                if (!string.IsNullOrEmpty(model.Nickname))
                {
                    this.SetOpenIdVal(this.OpenId);
                }
            }
            else
            {
                Response.Redirect(oauthUrl);
            }
        }



        public int CurrentCustomerID { get; set; }
        private void SetOpenIdVal(string openid)
        {
            CookieHelper.SetCookieVal(this.GetOpenIdKey(), 60 * 24 * 7, openid);
        }

        public string GetOpenIdVal()
        {
            return CookieHelper.GetCookieVal(this.GetOpenIdKey());
        }

        private string GetOpenIdKey()
        {
            return "bargin_userinfo_openid";
        }
        /// <summary>
        /// 默认用户授权
        /// </summary>
        public int scope
        {
            get
            {
                return this.GetQueryString("scope", 0);
            }
        }
        public string NoticeText { get; set; }
        public string OpenId
        {
            get;
            set;
        }




        ///// <summary>
        ///// 获取微信授权用户信息
        ///// </summary>
        ///// <param name="customerId">商户Id</param>
        ///// <returns></returns>
        //public WeixinOAuthUserInfoModel GetUserInfo(int customerId)
        //{
        //    string keyUserinfo = this.GetUserinfoDataKey(customerId);
        //    string encryptedUserInfo = CookieHelper.GetCookieVal(keyUserinfo);
        //    if (string.IsNullOrEmpty(encryptedUserInfo))
        //    {
        //        //尝试从session中读取
        //        if (HttpContext.Current.Session[keyUserinfo] != null)
        //        {
        //            WeixinOAuthUserInfoModel seModel = HttpContext.Current.Session[keyUserinfo] as WeixinOAuthUserInfoModel;
        //            return seModel;
        //        }
        //        return null;
        //    }
        //    try
        //    {
        //        WeixinOAuthUserInfoModel model = JsonConvert.DeserializeObject<WeixinOAuthUserInfoModel>(EncryptHelper.Decrypt(encryptedUserInfo, ENCRYPTKEY));
        //        return model;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Write(string.Format("WeixinOAuthUserDataProvider->GetUserInfo发生异常：{0}", ex.Message));
        //    }
        //    return null;
        //}

        //private string GetUserinfoDataKey(int customerId)
        //{
        //    return "wxoauth_uinfo_" + customerId;
        //}
    }
}