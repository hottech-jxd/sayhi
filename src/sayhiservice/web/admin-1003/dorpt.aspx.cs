using common;
using logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace admin_1003
{
    public partial class dorpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = UserSignUp.Instance.GetUserSignList(1003);
            if (dt != null)
            {
                string fileName = string.Format("download_{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
                //ExcelHelper.ExportDTtoExcel(dt, fileName);
                using (MemoryStream ms = ExcelHelper.ExportDT(dt))
                {
                    Response.ClearContent();
                    Response.ContentType = "application/ms-excel";
                    Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
                    Response.BinaryWrite(ms.ToArray());
                }
            }
        }
    }
}