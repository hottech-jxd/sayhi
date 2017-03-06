using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using logic.Model;
using logic.DAL;


namespace logic
{
    public class TaskService
    {
        private static TaskService _Instance = null;
        private readonly TaskDAL taskDal = new TaskDAL();
        private readonly LocationDAL locationDal = new LocationDAL();
        private readonly DeviceDAL deviceDal = new DeviceDAL();

        /// <summary>
        /// 单例出口
        /// </summary>
        public static TaskService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new TaskService();
                }
                return _Instance;
            }
        }


        public  ResultModel<TaskModel> GetTaskInfo( int taskid ){
            ResultModel<TaskModel> result=new ResultModel<TaskModel>();

            TaskModel task = taskDal.GetTask(taskid);
            if(task==null) {
                result.code = Constants.SUCCESS_CODE;
                result.message="没有满足条件的任务数据";
                result.data=null;
                return result;
            }

            if (task.deviceno != null && !string.IsNullOrEmpty(task.deviceno))
            {
                DeviceModel device = deviceDal.GetDeviceByDeviceNo(task.deviceno);
                task.device = device;
            }


            result.code = Constants.SUCCESS_CODE;
            result.message = "";
            result.data = task;
            return result;
        }



        public ResultModel<TaskModel> GetTaskInfoApp(string deviceno,string starttime, string stoptime  ){
            ResultModel<TaskModel> result=new ResultModel<TaskModel>();

            DateTime dt_starttime = new DateTime();
            if( !DateTime.TryParse(starttime, out dt_starttime)){   
                result.code=Constants.ERROR_CODE;
                result.message="starttime参数错误";
                return result;
            }
            
            DateTime dt_stoptime = new DateTime();
            if( !DateTime.TryParse(stoptime, out dt_stoptime)){
                result.code = Constants.ERROR_CODE; 
                result.message="stoptime参数错误";
                return result;
            }

            //现查找 正在运行的任务，然后在找未开始的任务
            int status = Constants.TASKSTATUS_RUNNING;
            List<TaskModel> taskList = taskDal.GetTaskList(deviceno, dt_starttime, dt_stoptime , status );
            TaskModel task; 
            if (taskList !=null && taskList.Count > 0)
            {
                task = taskList[0];
            }
            else
            {
                status = Constants.TASKSTATUS_INIT;
                taskList = taskDal.GetTaskList(deviceno, dt_starttime, dt_stoptime, status);
                if ( taskList ==null || taskList.Count < 1)
                {
                    result.code = Constants.SUCCESS_CODE;
                    result.message = "没有满足条件的任务数据";
                    result.data = null;
                    return result;
                }
                task = taskList[0];
            }
               
            //TaskModel task = taskList[0];
            List<LocationModel> locations = locationDal.GetLocations(task.taskid, 0);
            task.locations = locations;

            result.code = Constants.SUCCESS_CODE;
            result.message = "";
            result.data = task;
            return result;
        }


        public TaskModel GetTaskModel(int taskid)
        {
            return taskDal.GetTask(taskid);
        }


        public ResultModel<TaskModel> AddTaskInfo(TaskModel model)
        {
            try
            {
                model.createtime = DateTime.Now;
                model.status = Constants.TASKSTATUS_INIT;

                int taskid = taskDal.AddTask(model);
                model.taskid = taskid;

                foreach (LocationModel item in model.locations)
                {
                    item.taskid = model.taskid;

                    locationDal.AddLocation(item);
                }

                ResultModel<TaskModel> result = new ResultModel<TaskModel>();
                result.code = Constants.SUCCESS_CODE;
                result.message = "新增成功";

                return result;
            }
            catch (Exception ex)
            {
                ResultModel<TaskModel> result = new ResultModel<TaskModel>();
                result.code = Constants.ERROR_CODE;
                result.message = ex.Message;

                return result;
            }
        }


        public List<TaskModel> GetTaskList(int pageIndex, int pageSize, out int recordCount, TaskWhere where)
        {
            List<TaskModel> list = taskDal.GetTaskListPage(pageIndex, pageSize, out recordCount, where);
            return list;
        }

        public void DeleteTask(int taskid)
        {
            int result1 = taskDal.DeleteTask(taskid);
            int result2 = locationDal.DeleteLocation(taskid);
             
        }

        bool ExistLocation(List<LocationModel> list, LocationModel model)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].longitude.Equals(model.longitude) && list[i].latitude.Equals(model.latitude))
                {
                    return true;
                }
            }

            return false;
        }


        public ResultModel<TaskModel> UpdateTaskInfo(TaskModel model)
        {
            try
            {              
                

                 List<LocationModel> locations = locationDal.GetLocations(model.taskid);
                 if (locations != null && locations.Count > 0)
                 {
                     for (int i = 0; i < locations.Count; i++)
                     {
                         if( !ExistLocation( model.locations , locations[i] ) ){
                             locationDal.DeleteLocationByLocationId(locations[i].locationid);
                         }
                     }
                 }


                 bool hasNewLcoation = false;

                foreach (LocationModel item in model.locations)
                {
                    if (locationDal.ExistByLatLng(item.latitude, item.longitude, model.taskid) < 1)
                    {
                        item.taskid = model.taskid;
                        locationDal.AddLocation(item);
                        hasNewLcoation = true;
                    }
                }

                //重新获得任务的最新状态
                TaskModel newTaskModel = taskDal.GetTask(model.taskid);                
                if ( newTaskModel.status == Constants.TASKSTATUS_FINISHED && hasNewLcoation)
                {//如果任务的最新状态是已经完成了，但是又添加了新的位置信息，则改变任务的状态为“未开始”
                    model.status = Constants.TASKSTATUS_INIT;
                    taskDal.UpdateTaskWithStatus(model);
                }
                else
                {//
                    taskDal.UpdateTaskWithNoStatus(model);
                }


                ResultModel<TaskModel> result = new ResultModel<TaskModel>();
                result.code = Constants.SUCCESS_CODE;
                result.message = "修改成功";

                return result;
            }
            catch (Exception ex)
            {
                ResultModel<TaskModel> result = new ResultModel<TaskModel>();
                result.code = Constants.ERROR_CODE;
                result.message = ex.Message;

                return result;
            }

        }


        public ResultModel<TaskModel> SetTaskDevice(int taskid, int deviceid )
        {
            ResultModel<TaskModel> result = new ResultModel<TaskModel>();
             DeviceModel deviceModel =  deviceDal.GetDeviceByDeviceId(deviceid);
             if (deviceModel == null)
             {
                 result.code = Constants.ERROR_CODE;
                 result.message = "设备不存在";
                 return result;
             }

             int count = taskDal.SetTaskDevice(taskid, deviceModel.deviceno);

             result.code = Constants.SUCCESS_CODE;
             result.message = "设置设备成功";
             return result;
        }


        public int UpdateLocationStatus(int taskid, int locationid, int status , string remark )
        {
            return taskDal.UpdateLocationStatus(taskid, locationid, status ,remark);
        }

        public int ClearTaskDevice(int taskid)
        {
            return taskDal.ClearTaskDevice(taskid);
        }

        public int UpdateTaskStatusRun(int taskid, int status)
        {
            return taskDal.UpdateTaskStatusRun(taskid, status);
        }
    
    }
}
