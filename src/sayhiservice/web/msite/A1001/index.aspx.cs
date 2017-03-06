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
    public partial class index : PageBase
    {
        protected string shareOpenId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            shareOpenId = this.GetOpenIdVal();
        }
    }
}