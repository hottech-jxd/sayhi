using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace logic.Model
{
    public class UserSignModel
    {
        public Int64 rowIndex { get; set; }
        public int id { get; set; }

        public int customerid { get; set; }

        public string name { get; set; }

        public string mobile { get; set; }

        public string nickname { get; set; }

        public string headimgurl { get; set; }

        public string city { get; set; }

        public string address { get; set; }

        public string openid { get; set; }

        public string Province { get; set; }

        public DateTime createtime { get; set; }
        /// <summary>
        /// 类型 0免费预约 1，抢零元设计
        /// </summary>
        public int Type { get; set; }

        public string content1 { get; set; }
        public string content2 { get; set; }
        public string content3 { get; set; }
        public string content4 { get; set; }


    }

    /// <summary>
    /// 用户搜索
    /// </summary>
    public class SignUserSearchWhere
    {
        public string key { get; set; }
        public string city { get; set; }
        public string pro { get; set; }
        public string area { get; set; }
    }
}
