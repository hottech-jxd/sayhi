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
            string _openid=this.GetQueryString("openid","");

            string callbakUrl = Server.UrlEncode(HttpContext.Current.Request.RawUrl);

            string oauthUrl = "http://yilite.huobanmall.com/OAuth2/WeixinAuthorize.aspx?retuinfo=&scope=0&customerid=6457&redirecturl=" + callbakUrl;

            if (string.IsNullOrEmpty(_openid))
            {
                Response.Redirect(oauthUrl);
            }
            else {
                UserService.Instance.SetUserInfo(this.CurrentCustomerID, new WeixinOAuthUserInfoModel()
                {
                    City = "",
                    Country = "",
                    Headimgurl = "",
                    Nickname = "",
                    Openid = StringHelper.RandomNo(new Random(Guid.NewGuid().GetHashCode()), 32),
                    Privilege = null,
                    Province = "",
                    Sex = 0,
                    UnionID = ""
                });
            }

            //用户授权
            WeixinOAuthUserInfoModel model = UserService.Instance.GetUserInfo(this.CurrentCustomerID);
            if (model != null)
            {
                //用户授权
                 model = UserService.Instance.GetUserInfo(this.CurrentCustomerID);
                if (model == null)
                {
                   
                }
            }
            else
            {
                
            }

            this.CurrentCustomerID = 1003;
           
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