using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace logic.Model
{
    public class ResultModel<T>
    {
        public int code { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}
