using common;
using logic;
using logic.Model;
using Newtonsoft.Json;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace msite.OAuth2
{
    /// <summary>
    /// 微信OAuth2.0回调（经过认证的服务号才能使用的高级接口）
    /// </summary>
    public partial class WeixinCallback : PageBaseHelper
    {
        string _redirectUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Code.Equals("authdeny", StringComparison.CurrentCultureIgnoreCase))
            {
                this.WriteLog("当前用户取消了授权。");
                return;
            }

            //合法的链接都会带上code和state
            if (this.Code == "" || this.State == "")
            {
                this.WriteLog("不合法的请求");
                return;
            }
            //分析state中的数据
            this.DoAnalyzeState();
            if (this.NonceStr == "" || this.CustomerId == 0)
            {
                this.WriteLog("state参数残缺");
                return;
            }

            _redirectUrl = UserService.Instance.GetOAuthUrl(this.NonceStr);

            //获取带openid的信息
            OAuthAccessTokenResult atResult;
            OAuthUserInfoPlus oauthUserInfo = null;
            try
            {
                string errmsg = string.Empty;
                //获取openid
                atResult = OAuth.GetAccessToken(this.AppId(this.CustomerId), this.AppSecret(this.CustomerId), this.Code);
                if (atResult.errcode != ReturnCode.请求成功)
                {
                    this.WriteLog(string.Format("code:{0},msg:{1}", atResult.errcode, atResult.errmsg));
                    return;
                }
                #region 使用高级授权获取用户信息
                try
                {
                    // oauthUserInfo = OAuth.GetUserInfo(atResult.access_token, atResult.openid);
                    //新方法
                    string wxApiUrl = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", atResult.access_token, atResult.openid);
                    MPUserInfoResult result = MPHelper.GetWxUserInfo(wxApiUrl, out errmsg);
                    if (result != null)
                    {
                        this.SetUserInfo(this.CustomerId, new WeixinOAuthUserInfoModel()
                         {
                             City = result.city,
                             Country = result.country,
                             Headimgurl = result.headimgurl,
                             Nickname = result.nickname,
                             Openid = result.openid,
                             Privilege = result.privilege,
                             Province = result.province,
                             Sex = result.sex,
                             UnionID = result.unionid
                         });
                        oauthUserInfo = new OAuthUserInfoPlus()
                        {
                            city = result.city,
                            country = result.country,
                            headimgurl = result.headimgurl,
                            nickname = result.nickname,
                            openid = result.openid,
                            privilege = result.privilege,
                            province = result.province,
                            sex = result.sex,
                            unionid = result.unionid
                        };
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Write("OAuth.GetAccessToken:" + ex.Message);
                    this.WriteLog(ex.Message);
                    return;
                }


                try
                {
                    string url = "";
                    #region 1、openid处理带上URL
                    url = this.AppendUrlParameters(_redirectUrl, "openid", atResult.openid);
                    #endregion

                    #region 2、授权用户的详细信息JSON带上URL
                    if (this.ReturnUserInfo && oauthUserInfo != null)
                    {
                        string encodedUserInfo = HttpContext.Current.Server.UrlEncode(JsonConvert.SerializeObject(oauthUserInfo));
                        url = this.AppendUrlParameters(url, "retuinfo", encodedUserInfo);
                    }
                    if (TryReg)
                    {
                        WeixinOAuthUserInfoModel wouInfo = null;
                        if (oauthUserInfo != null)
                        {
                            wouInfo = GetUserInfo(this.CustomerId);
                        }
                        int retuid = UserService.Instance.GetTopOAuthedUserId(this.CustomerId, atResult.openid);
                        if (retuid == 0)//注册
                        {
                            /**
                             * 执行用户注册操作
                             */
                            retuid = UserService.Instance.RegisterUser(this.CustomerId, wouInfo);
                        }
                        url = this.AppendUrlParameters(url, "retuid", retuid.ToString());
                    }

                    this.StoreOpenId(this.CustomerId, atResult.openid);
                    this.SetOpenId(this.CustomerId, atResult.openid);


                    Response.Redirect(url, false);

                    #endregion
                }
                catch (Exception)
                {

                    throw;
                }



                #endregion

            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 得到时间戳
        /// </summary>
        /// <returns></returns>
        private string GetTimeStamp()
        {
            return ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
        }
        /// <summary>
        /// 保存微信授权用户信息
        /// </summary>
        /// <param name="customerId">商户Id</param>
        /// <param name="model"></param>
        public void SetUserInfo(int customerId, WeixinOAuthUserInfoModel model)
        {
            string json = JsonConvert.SerializeObject(model);
            string jsonEncrypt = EncryptHelper.Encrypt(json, ENCRYPTKEY);
            CookieHelper.SetCookieValByCurrentDomain(this.GetUserinfoDataKey(customerId), 1, jsonEncrypt);

            //双保险，session也存储
            HttpContext.Current.Session[this.GetUserinfoDataKey(customerId)] = model;
        }
        /// <summary>
        /// 保存OPenID 到Cookie
        /// </summary>
        /// <param name="customerid">商户ID</param>
        /// <param name="openid">唯一标示</param>
        public void StoreOpenId(int customerid, string openid)
        {
            HttpContext.Current.Session[this.GetOpenIdKey(customerid)] = openid;
        }
        /// <summary>
        /// 保存微信openid
        /// </summary>
        /// <param name="customerId">商户Id</param>
        /// <param name="openId"></param>
        public void SetOpenId(int customerId, string openId)
        {
            CookieHelper.SetCookieValByCurrentDomain(this.GetOpenIdDataKey(customerId), 1, EncryptHelper.Encrypt(openId, ENCRYPTKEY));

            //双保险，session也存储
            HttpContext.Current.Session[this.GetOpenIdDataKey(customerId)] = openId;
        }

        private string GetUserinfoDataKey(int customerId)
        {
            return "wxoauth_uinfo_" + customerId;
        }
        private string GetOpenIdDataKey(int customerId)
        {
            return "wxoauth_id_" + customerId;
        }
        private string GetOpenIdKey(int customerid)
        {
            return "oauth_openid_" + customerid;
        }

        /// <summary>
        /// URL上追加指定参数，如存在则覆盖
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pname"></param>
        /// <param name="pvalue"></param>
        /// <returns></returns>
        private string AppendUrlParameters(string url, string pname, string pvalue)
        {
            if (url.IndexOf("?") != -1)
            {
                if (url.IndexOf(string.Format("?{0}=", pname), StringComparison.CurrentCultureIgnoreCase) > 0
                    || url.IndexOf(string.Format("&{0}=", pname), StringComparison.CurrentCultureIgnoreCase) > 0)
                {
                    string regexstr = string.Format(@"[\?|&](?<param>{0}=[^&]*)([\&]?).*$", pname);//替换原URL中的参数
                    Match ma = Regex.Match(_redirectUrl, regexstr, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    if (ma.Success)
                    {
                        url = url.Replace(ma.Groups["param"].Value, pname + "=" + pvalue);
                    }
                }
                else
                {
                    url = url + "&" + pname + "=" + pvalue;
                }
            }
            else
            {
                url = url + "?" + pname + "=" + pvalue;
            }
            return url;
        }

        /// <summary>
        /// 从STATE参数中分析出去微信服务器授权前带过来的信息
        /// </summary>
        private void DoAnalyzeState()
        {
            if (this.State.Length > 5)
            {
                this.NonceStr = this.State.Substring(0, 8);
                this.CustomerId = Convert.ToInt32(this.State.Substring(8, 8).TrimStart('0'));
                this.OAuthScopeType = (this.State.Substring(16, 1) == "1" ? OAuthScope.snsapi_userinfo : OAuthScope.snsapi_base);
                this.GetUserInfoByGlobalAccessToken = this.State.Substring(18, 1) == "1";
                this.ReturnUserInfo = this.State.Substring(19, 1) == "1";
                this.TryReg = this.State.Substring(20, 1) == "1";
            }
        }

        private void WriteLog(string str)
        {
            string content = this.LoadErrorNote((int)ErrorPageOptions.报错, str);
            Response.Write(content);
        }



        /// <summary>
        /// 获取微信授权用户信息
        /// </summary>
        /// <param name="customerId">商户Id</param>
        /// <returns></returns>
        public WeixinOAuthUserInfoModel GetUserInfo(int customerId)
        {
            string keyUserinfo = this.GetUserinfoDataKey(customerId);
            string encryptedUserInfo = CookieHelper.GetCookieVal(keyUserinfo);
            if (string.IsNullOrEmpty(encryptedUserInfo))
            {
                //尝试从session中读取
                if (HttpContext.Current.Session[keyUserinfo] != null)
                {
                    WeixinOAuthUserInfoModel seModel = HttpContext.Current.Session[keyUserinfo] as WeixinOAuthUserInfoModel;
                    return seModel;
                }
                return null;
            }
            try
            {
                WeixinOAuthUserInfoModel model = JsonConvert.DeserializeObject<WeixinOAuthUserInfoModel>(EncryptHelper.Decrypt(encryptedUserInfo, ENCRYPTKEY));
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("WeixinOAuthUserDataProvider->GetUserInfo发生异常：{0}", ex.Message));
            }
            return null;
        }



        #region URL参数
        /// <summary>
        /// 只获取openid，不做注册登录操作
        /// </summary>
        public bool OnlyGetOpenID
        {
            get;
            private set;
        }

        /// <summary>
        /// 商户ID
        /// </summary>
        public int CustomerId
        {
            get;
            private set;
        }

        /// <summary>
        /// Session key
        /// </summary>
        public string NonceStr
        {
            get;
            private set;
        }

        /// <summary>
        /// 授权类型
        /// </summary>
        public OAuthScope OAuthScopeType
        {
            get;
            private set;
        }

        /// <summary>
        /// 当该项启用时间，将通过“全局Access Token获取用户基本信息”，
        /// 优先级高于“通过OAuth2.0方式弹出授权页面获得用户基本信息”，（OAuthScope.snsapi_userinfo）,
        /// 获取用户信息建议使用snsapi_base获取openid,然后再用GlobalAccessToken方式获取用户信息，不会出现确认框
        /// </summary>
        public bool GetUserInfoByGlobalAccessToken
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否返回用户授权的详细信息,目前只在scope=1时有效
        /// </summary>
        public bool ReturnUserInfo { get; private set; }

        /// <summary>
        /// 是否尝试注册
        /// </summary>
        public bool TryReg { get; private set; }

        /// <summary>
        /// 微信返回的code，获取accessToken时需用到
        /// </summary>
        public string Code
        {
            get
            {
                return this.GetQueryString("code", "");
            }
        }

        /// <summary>
        /// 微信返回的state
        /// </summary>
        public string State
        {
            get
            {
                return this.GetQueryString("state", "");
            }
        }
        #endregion

    }
}