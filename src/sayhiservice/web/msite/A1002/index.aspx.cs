using logic;
using logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace msite.A1002
{
    public partial class index : BaseClass
    {

        protected override void OnPreInit(EventArgs e)
        {
            this.LoadInit(1001, false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            IsSignUp = 0;
            WeixinOAuthUserInfoModel userModel = UserService.Instance.GetUserInfo(this.CurrentCustomerID);
            if (userModel != null)
            {
                data = UserSignUp.Instance.GetSignUpUserInfo(CurrentCustomerID, userModel.Openid,0);
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