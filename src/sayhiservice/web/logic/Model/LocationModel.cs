using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace logic.Model
{
    public class LocationModel
    {
        public int locationid { get; set; }
        public int taskid { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string address { get; set; }
        public int status { get; set; }

        public string remark { get; set; }
    }
}
