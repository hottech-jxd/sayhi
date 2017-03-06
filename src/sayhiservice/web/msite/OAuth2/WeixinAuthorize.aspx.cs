using common;
using logic;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace msite.OAuth2
{
    public partial class WeixinAuthorize : PageBaseHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //组装好去微信授权的链接，跳转
            //state:随机码(8位)+商户ID(8位)+授权类型(1位)+是否使用火图的服务号(1位)
            //+是否用全局令牌获取用户(1位)+是否返回详细的授权用户json(1位)+是否注册新用户(1位)
            string nonceStr = StringHelper.CreateCheckCode(8);
            string state = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                nonceStr,
                this.CustomerID.ToString().PadLeft(8, '0'),
                this.OAuthScopeType == OAuthScope.snsapi_base ? "0" : "1",
                "0",
                this.GetUserInfoByGlobalAccessToken ? "1" : "0",
                this.ReturnUserInfo ? "1" : "0",
                this.TryReg ? "1" : "0");

            string redirectUrl = this.RedirectUrl.Replace("^^", "-");
            //存储，从微信回来时用
            UserService.Instance.AddOAuthUrl(nonceStr, redirectUrl);

            string callbackUrl = string.Format("{0}/OAuth2/WeixinCallback.aspx", "http://" + Request.Url.Authority);
            string url = OAuth.GetAuthorizeUrl(this.AppId(this.CustomerID), callbackUrl, state, this.OAuthScopeType);
            Response.Redirect(url, true);
        }
        /// <summary>
        /// 商户ID
        /// </summary>
        public int CustomerID
        {
            get
            {
                return this.GetQueryString("customerid", 0);
            }
        }
        /// <summary>
        /// 授权方式，默认为base方式
        /// </summary>
        public OAuthScope OAuthScopeType
        {
            get
            {
                return this.GetQueryString("scope", 0) == 0 ? OAuthScope.snsapi_base : OAuthScope.snsapi_userinfo;
            }
        }

        /// <summary>
        /// 授权完成后要去的地址
        /// </summary>
        public string RedirectUrl
        {
            get
            {
                return this.GetQueryString("redirecturl", "");
            }
        }
        /// <summary>
        /// 是否返回用户授权的详细信息,目前只在scope=1时有效
        /// </summary>
        public bool ReturnUserInfo
        {
            get
            {
                return this.GetQueryString("retuinfo", 0) == 1;
            }
        }
        /// <summary>
        /// 当该项启用时间，将通过“全局Access Token获取用户基本信息”，
        /// 优先级高于“通过OAuth2.0方式弹出授权页面获得用户基本信息”，（OAuthScope.snsapi_userinfo）,
        /// 获取用户信息建议使用snsapi_base获取openid,然后再用GlobalAccessToken方式获取用户信息，不会出现确认框
        /// </summary>
        public bool GetUserInfoByGlobalAccessToken
        {
            get
            {
                return this.GetQueryString("userinfo", 0) == 1;
            }
        }
        /// <summary>
        /// 是否尝试注册
        /// </summary>
        public bool TryReg
        {
            get
            {
                return this.GetQueryString("tryreg", 1) == 1;
            }
        }
    }
}