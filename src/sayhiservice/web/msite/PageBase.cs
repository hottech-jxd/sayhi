using common;
using logic.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace msite
{
    public class PageBase : BaseClass
    {
        protected override void OnPreInit(EventArgs e)
        {
            this.LoadInit(1001);
        }
    }
}