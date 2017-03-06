using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace logic.Model
{
    public  class DeviceModel
    {
        public int deviceid { get; set; }
        public string devicename { get; set; }

        public string deviceno { get; set; }

        public string osversion { get; set; }

        public string devicetype { get; set; }

        public string brand { get; set; }

        public DateTime createtime { get; set; }

        public string remark { get; set; }
    }


    public class DeviceWhere
    {
        public string deviceno { get; set; }
        public string deviename { get; set; }
        public string osversion { get; set; }
        public string brand { get; set; }


    }
}
