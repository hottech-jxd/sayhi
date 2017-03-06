using common;
using logic;
using logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace msite.A1003
{
    public partial class index : BaseClass
    {

        protected override void OnPreInit(EventArgs e)
        {


            string callbakUrl = Server.UrlEncode(HttpContext.Current.Request.RawUrl);

            string oauthUrl = "/OAuth2/WeixinAuthorize.aspx?userinfo=0&scope=1&uda=0&customerid=" + this.CurrentCustomerID + "&tryreg=" + (tryreg ? 1 : 0) + "&redirecturl=" + callbakUrl;

            //用户授权
            WeixinOAuthUserInfoModel model = UserService.Instance.GetUserInfo(this.CurrentCustomerID);
            if (model != null)
            {
               
            }
            else
            {
                Response.Redirect(oauthUrl);
            }

            this.CurrentCustomerID = 1003;
            //用户授权
            WeixinOAuthUserInfoModel model = UserService.Instance.GetUserInfo(this.CurrentCustomerID);
            if (model == null)
            {
                UserService.Instance.SetUserInfo(this.CurrentCustomerID, new WeixinOAuthUserInfoModel()
                {
                    City = "",
                    Country ="",
                    Headimgurl ="",
                    Nickname = "",
                    Openid = StringHelper.RandomNo(new Random(Guid.NewGuid().GetHashCode()), 32),
                    Privilege = null,
                    Province = "",
                    Sex = 0,
                    UnionID = ""
                });
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            IsSignUp = 0;
            WeixinOAuthUserInfoModel userModel = UserService.Instance.GetUserInfo(this.CurrentCustomerID);
            if (userModel != null)
            {
                data = UserSignUp.Instance.GetSignUpUserInfo(CurrentCustomerID, userModel.Openid, 0);
                if (data != null)
                {
                    IsSignUp = 1;
                }


            }
        }

        public int IsSignUp { get; set; }
        /// <summary>
        /// 报名用户数据
        /// </summary>
        public UserSignModel data { get; set; }
    }
}