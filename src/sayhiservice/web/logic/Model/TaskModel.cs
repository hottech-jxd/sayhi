using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace logic.Model
{
    public class TaskModel
    {
        public int taskid { get; set; }
        public DateTime starttime { get; set; }
        public DateTime stoptime { get; set; }
        public string sayhi { get; set; }
        public int status { get; set; }
        public int sayhirate { get; set; }

        public int sayhimaxcount { get; set; }

        public DateTime createtime { get; set; }

        public string deviceno { get; set; }

        public string remark { get; set; }

        public List<LocationModel> locations { get; set; }

        public DeviceModel device { get; set; }
    }

    public class TaskWhere
    {
        public string starttime { get; set; }
        public string status { get; set; }
        public string deviceno { get; set; }


    }
}
