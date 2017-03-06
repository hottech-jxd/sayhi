using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace logic.Model
{
    public class Constants
    {
        public static int SUCCESS_CODE = 200;
        public static int ERROR_CODE = 500;

        public static int TASKSTATUS_INIT = 0;
        public static int TASKSTATUS_RUNNING = 1;
        public static int TASKSTATUS_FINISHED = 8;

        public static int SUBTASK_INIT = 0;
        public static int SUBTASK_RUNNING = 1;
        public static int SUBTASK_FINISHED = 8;
    }
}
