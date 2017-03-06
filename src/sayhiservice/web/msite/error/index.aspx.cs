using common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace msite.error
{
    public partial class index : PageBaseHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string content = this.LoadErrorNote((int)ErrorPageOptions.报错, note);
            Response.Write(content);      
        }


        public string note {
            get {
                return this.GetQueryString("t","");
            }
        }
    }
}