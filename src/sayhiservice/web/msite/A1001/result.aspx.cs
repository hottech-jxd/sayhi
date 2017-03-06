using logic;
using logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace msite.A1001
{
    public partial class result : PageBase
    {
        protected AttendUserModel userModel = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            int shopid = UserService.Instance.GetShopId(CurrentCustomerID, this.GetOpenIdVal());
            if (shopid > 0)
            {
                userModel = UserService.Instance.GetUserCouponInfo(CurrentCustomerID, couponCode);
                if (UserService.Instance.ShopUseCoupon(CurrentCustomerID, shopid, couponCode) > 0)
                {
                    isSuccess = true;
                    showText = "使用成功";
                }
                else
                {
                    isSuccess = false;
                    showText = "优惠券已使用";
                }
            }
            else
            {
                isSuccess = false;
                showText = "您不是经销商，请去绑定账号！";
            }
        }


        public string couponCode
        {
            get
            {
                return GetQueryString("code", "");
            }
        }

        public bool isSuccess { get; set; }


        public string showText { get; set; }
    }
}