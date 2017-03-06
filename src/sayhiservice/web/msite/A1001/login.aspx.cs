using logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace msite.A1001
{
    public partial class login : PageBase
    {
        protected string currentOpenid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            currentOpenid = this.GetOpenIdVal();
            if (!IsPostBack)
            {
                int shopid = UserService.Instance.GetShopId(CurrentCustomerID, this.GetOpenIdVal());
                if (shopid > 0)//你已绑定过
                {
                    Response.Redirect("success.html");
                }
            }
        }
    }
}