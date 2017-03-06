using common;
using logic;
using logic.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using logic.Model;

namespace admin
{
    public partial class AjaxHandler : IBaseHandler
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/json";
            this.Data = new Dictionary<object, object>();
            this.OperatePostMethod();
            this.Data["code"] = this.code;
            this.Data["message"] = this.message;

            //ResultModel<Object> result = new logic.Model.ResultModel<object>();
            //result.code = this.code;
            //result.message = this.msg;
            //this.Data["data"] = result;
            

            this.outputJson = this.GetJson(this.Data);
            Response.Write(this.outputJson);
        }
        private void OperatePostMethod()
        {
            try
            {
                switch (this.Action)
                {
                    case "login":
                        login();
                        break;
                    case "islogin":
                        isLogin();
                        break;
                    case "logout":
                        logout();
                        break;
                    //case "usersignlist"://报名用户
                    //    UserSignList();
                    //    break;
                    case "adddevice"://
                        addDevice();
                        break;
                    case "getdevice":
                        GetDevice();
                        break;
                    case "devicelist":
                        DeviceList();
                        break;
                    case "deletedevice":
                        DeleteDevice();
                        break;
                    case "updatedevice":
                        UpdateDevice();
                        break;
                    case "addtask":
                        AddTask();
                        break;
                    case "gettaskinfo":
                        GetTaskInfoApp();
                        break;
                    case "tasklist":
                        TaskList();
                        break;
                    case "deletetask":
                        DeleteTask();
                        break;
                    case "edittask":
                        EditTask();
                        break;
                    case "updatetask":
                        UpdateTask();
                        break;
                    case "settaskdevice":
                        SetTaskDevice();
                        break;
                    case "updatelocationstatus":
                        UpdateLocationStatus();
                        break;
                    case "cleartaskdevice":
                        ClearTaskDevice();
                        break;
                    case "updatetaskstatusrun":
                        UpdateTaskStatusRun();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
        }


        private void login()
        {
            string loginName = this.GetFormValue("loginName", "");//登录名
            string loginPwd = this.GetFormValue("loginPwd", "");//登录密码

            string _ln = ConfigHelper.GetConfigString("loginName", "");
            string _lp = ConfigHelper.GetConfigString("loginPwd", "");
            if (loginName == _ln && loginPwd == _lp)
            {
                this.code = 1;
                string json = string.Format("{0}|{1}", loginName, loginPwd);
                string jsonEncrypt = EncryptHelper.Encrypt(json, ENCRYPTKEY);
                CookieHelper.SetCookieValByCurrentDomain(this.GetUserinfoDataKey(), 1, jsonEncrypt);

                //双保险，session也存储
                HttpContext.Current.Session[this.GetUserinfoDataKey()] = jsonEncrypt;
            }
        }

        private string GetUserinfoDataKey()
        {
            return "admin_uinfo";
        }
        /// <summary>
        /// 检查登录
        /// </summary>        
        /// <returns></returns>
        public void isLogin()
        {
            string keyUserinfo = this.GetUserinfoDataKey();
            string encryptedUserInfo = CookieHelper.GetCookieVal(keyUserinfo);
            if (string.IsNullOrEmpty(encryptedUserInfo))
            {
                //尝试从session中读取
                if (HttpContext.Current.Session[keyUserinfo] != null)
                {
                    encryptedUserInfo = HttpContext.Current.Session[keyUserinfo].ToString();
                }
            }
            try
            {
                string userInfo = EncryptHelper.Decrypt(encryptedUserInfo, ENCRYPTKEY);
                string[] arr = userInfo.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                string _ln = ConfigHelper.GetConfigString("loginName", "");
                string _lp = ConfigHelper.GetConfigString("loginPwd", "");
                if (arr.Length > 1)
                {
                    if (arr[0] == _ln && arr[1] == _lp)
                        this.code = 1;
                }
            }
            catch (Exception)
            {
                this.code = 0;
            }
        }

        private void logout()
        {
            CookieHelper.SetCookieValByCurrentDomain(this.GetUserinfoDataKey(), 1, string.Empty);
            //双保险，session也存储
            HttpContext.Current.Session[this.GetUserinfoDataKey()] = string.Empty;
        }


        /// <summary>
        /// 获取报名用户
        /// </summary>
        private void UserSignList()
        {
            int pageindex = this.GetFormValue("page", 1);//
            int pageSize = this.GetFormValue("pagesize", 20);
            int recordCount = 0;
            SignUserSearchWhere where = new SignUserSearchWhere();
            where.key = this.GetFormValue("name", "");
            where.area = this.GetFormValue("area", "");
            where.pro = this.GetFormValue("pro", "");
            where.city = this.GetFormValue("city", "");
            List<UserSignModel> result = UserSignUp.Instance.GetUserSignList(customerid, pageindex, pageSize, out recordCount, where);
            int pageCount = recordCount / pageSize;
            if (recordCount % pageSize != 0)
            {
                ++pageCount;
            }
            this.Data["PageSize"] = pageSize;
            this.Data["PageIndex"] = pageindex;
            this.Data["Total"] = recordCount;
            this.Data["PageCount"] = pageCount;
            this.Data["Rows"] = result;
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeviceList( )
        {
            int pageindex = this.GetFormValue("page", 1);//
            int pageSize = this.GetFormValue("pagesize", 20);
            int recordCount = 0;
            DeviceWhere where = new DeviceWhere();
            where.brand=this.GetFormValue("brand","");
            where.deviceno=this.GetFormValue("deviceno","");
            where.deviename = this.GetFormValue("devicename","");
            where.osversion=this.GetFormValue("osversion","");
            

            List<DeviceModel> result = DeviceService.Instance.DeviceList(pageindex , pageSize , out recordCount , where);
            int pageCount = recordCount / pageSize;
            if (recordCount % pageSize != 0)
            {
                ++pageCount;
            }
            this.Data["PageSize"] = pageSize;
            this.Data["PageIndex"] = pageindex;
            this.Data["Total"] = recordCount;
            this.Data["PageCount"] = pageCount;
            this.Data["Rows"] = result;

        }


        public void addDevice()
        {
            string deviceName = this.GetFormValue("devicename", "");
            string deviceNo = this.GetFormValue("deviceno","");
            string osversion = this.GetFormValue("osversion","");
            string devicetype = this.GetFormValue("devicetype","");
            string brand = this.GetFormValue("brand", "");

            ResultModel<DeviceModel> result;

            if (string.IsNullOrEmpty(deviceNo))
            {
                result = new ResultModel<DeviceModel>();
                result.code = logic.Model.Constants.ERROR_CODE;
                result.message = "设备号空";
                this.code = result.code;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }

            result = DeviceService.Instance.AddDevice(deviceName, deviceNo, osversion, devicetype,brand);
            this.code = result.code;
            this.message = result.message;
            this.Data["data"] = result;
        }


        public void DeleteDevice()
        {
            int deviceid = this.GetFormValue("deviceid", 0);
            ResultModel<DeviceModel> result = new logic.Model.ResultModel<logic.Model.DeviceModel>();

            if (deviceid < 1)
            {
                result.code = Constants.ERROR_CODE;
                result.message = "参数错误";
                this.code = result.code;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }

            int count = DeviceService.Instance.DeleteDevice(deviceid);
            result.code = Constants.SUCCESS_CODE;
            result.message = "删除成功";
            this.code = result.code;
            this.message = result.message;
            this.Data["data"] = result;
            return;
        }


        public void GetDevice()
        {
            int deviceid = this.GetFormValue("deviceid", 0);

            DeviceModel model = DeviceService.Instance.GetDevice(deviceid);
            if (model == null)
            {
                this.code = Constants.ERROR_CODE;
                this.message = "无法获得信息";
                this.Data["data"] = "";
                return;
            }

            this.code = Constants.SUCCESS_CODE;
            this.message = "成功";
            this.Data["data"] = model;
        }

        public void UpdateDevice()
        {
            int id = this.GetFormValue("deviceid", 0);
            string remark = GetFormValue("remark", "");

            int count = DeviceService.Instance.UpdateDevice(id, remark);

            ResultModel<DeviceModel> result = new ResultModel<DeviceModel>();
            result.code = Constants.SUCCESS_CODE;
            result.message = "修改成功";

            this.code = result.code;
            this.message = result.message;
            this.Data["data"] = result;
        }

        public void AddTask()
        {
            string taskjson = this.GetFormValue("task", "");
            ResultModel<TaskModel> result = new logic.Model.ResultModel<logic.Model.TaskModel>();


            if (string.IsNullOrEmpty(taskjson))
            {
                result.code = Constants.ERROR_CODE;
                result.message="参数空错误";
                this.code = Constants.ERROR_CODE;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }         

            TaskModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskModel>(taskjson);
            if (model == null)
            {
                result.code = Constants.ERROR_CODE;
                result.message = "参数格式错误";
                this.code = Constants.ERROR_CODE;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }

            if (string.IsNullOrEmpty(model.sayhi))
            {
                result.code = Constants.ERROR_CODE;
                result.message = "内容空";
                this.code = Constants.ERROR_CODE;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }
            if (model.locations == null || model.locations.Count < 1)
            {
                result.code = Constants.ERROR_CODE;
                result.message = "经纬度信息空";
                this.code = Constants.ERROR_CODE;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }

            if (DateTime.Compare(model.starttime, model.stoptime) >= 0)
            {
                result.code = Constants.ERROR_CODE;
                result.message = "结束时间不能小于开始时间";
                this.code = result.code;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }


           result = TaskService.Instance.AddTaskInfo(model);

           this.code = result.code;
           this.message = result.message;
            this.Data["data"] = result;
            return;
        }

        public void EditTask()
        {
            int taskid = this.GetFormValue("taskid", 0);
            ResultModel<TaskModel> result = new logic.Model.ResultModel<logic.Model.TaskModel>();
            if (taskid==0)
            {
                result.code = Constants.ERROR_CODE;
                result.message = "参数值空";
                this.code = result.code;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }

            result = TaskService.Instance.GetTaskInfo( taskid );
            this.code = result.code;
            this.message = result.message;
            this.Data["data"] = result;
            return;

        }


        public void GetTaskInfoApp()
        {
            string deviceno = this.GetFormValue("deviceno", "");
            string starttime = this.GetFormValue("starttime", "");
            string stoptime = this.GetFormValue("stoptime", "");
            int status = this.GetFormValue("status", Constants.TASKSTATUS_INIT );

            ResultModel<TaskModel> result =  new ResultModel<TaskModel>();
            if (string.IsNullOrEmpty(deviceno))
            {
                result.code = Constants.ERROR_CODE;
                result.message = "设备号空";
                this.code = result.code;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }
            if (string.IsNullOrEmpty(starttime))
            {
                result.code = Constants.ERROR_CODE;
                result.message = "开始时间空";
                this.code = result.code;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }
            if (string.IsNullOrEmpty(stoptime))
            {
                result.code = Constants.ERROR_CODE;
                result.message = "结束时间空";
                this.code = result.code;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }

            result = TaskService.Instance.GetTaskInfoApp(deviceno, starttime, stoptime  );
            this.code = result.code;
            this.message = result.message;
            this.Data["data"] = result.data;
            return;

        }


        public void TaskList()
        {
            string deviceno = this.GetFormValue("deviceno", "");
            string starttime = this.GetFormValue("starttime", "");
            int pageindex = this.GetFormValue("page", 1);
            int pageSize = this.GetFormValue("pagesize", 20);
            int recordCount = 0;
            TaskWhere where = new TaskWhere();
            where.deviceno = this.GetFormValue("deviceno", "");
            where.starttime = this.GetFormValue("starttime", "");
            where.status = this.GetFormValue("status","");

            List<TaskModel> result = TaskService.Instance.GetTaskList(pageindex, pageSize, out recordCount, where);
            int pageCount = recordCount / pageSize;
            if (recordCount % pageSize != 0)
            {
                ++pageCount;
            }
            this.Data["PageSize"] = pageSize;
            this.Data["PageIndex"] = pageindex;
            this.Data["Total"] = recordCount;
            this.Data["PageCount"] = pageCount;
            this.Data["Rows"] = result;


        }


        public void DeleteTask()
        {
            int taskid = this.GetFormValue("taskid", -1);

            ResultModel<TaskModel> result = new logic.Model.ResultModel<logic.Model.TaskModel>();
            result = TaskService.Instance.GetTaskInfo(taskid);
            if ( result.code != Constants.SUCCESS_CODE )
            {
                this.code = result.code;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }

            if (result.data.status == Constants.TASKSTATUS_RUNNING)
            {
                result = new logic.Model.ResultModel<logic.Model.TaskModel>();
                result.code =  Constants.ERROR_CODE;
                result.message="当前任务正在运行,不能删除";
                this.code =result.code;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }

            if (result.data.status == Constants.TASKSTATUS_FINISHED)
            {
                result = new logic.Model.ResultModel<logic.Model.TaskModel>();
                result.code =  Constants.ERROR_CODE;
                result.message = "任务已经完成，不能删除";
                this.code = result.code;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }


            TaskService.Instance.DeleteTask(taskid);
                
            result.code = Constants.SUCCESS_CODE;
            result.message = "删除成功";
            this.code = result.code;
            this.message = result.message;
            this.Data["data"] = result;
        }


        public void UpdateTask()
        {
            string taskjson = this.GetFormValue("task", "");
            ResultModel<TaskModel> result = new logic.Model.ResultModel<logic.Model.TaskModel>();


            if (string.IsNullOrEmpty(taskjson))
            {
                result.code = Constants.ERROR_CODE;
                result.message = "参数空错误";
                this.code = Constants.ERROR_CODE;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }

            TaskModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskModel>(taskjson);
            if (model == null)
            {
                result.code = Constants.ERROR_CODE;
                result.message = "参数格式错误";
                this.code = Constants.ERROR_CODE;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }

            if (string.IsNullOrEmpty(model.sayhi))
            {
                result.code = Constants.ERROR_CODE;
                result.message = "打招呼内容空";
                this.code = Constants.ERROR_CODE;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }
            if (model.locations == null || model.locations.Count < 1)
            {
                result.code = Constants.ERROR_CODE;
                result.message = "经纬度信息空";
                this.code = Constants.ERROR_CODE;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }

            if (DateTime.Compare(model.starttime, model.stoptime) >= 0)
            {
                result.code = Constants.ERROR_CODE;
                result.message = "结束时间不能小于开始时间";
                this.code = result.code;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }


            result = TaskService.Instance.UpdateTaskInfo(model);

            this.code = result.code;
            this.message = result.message;
            this.Data["data"] = result;
            return;
        }


        public void SetTaskDevice()
        {
            ResultModel<TaskModel> result = new logic.Model.ResultModel<logic.Model.TaskModel>();         
            int taskid = this.GetFormValue("taskid", 0);
            int deviceid = this.GetFormValue("deviceid", 0);
            if (taskid == 0)
            {
                result.code = Constants.ERROR_CODE;
                result.message = "参数错误";
                this.code = result.code;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }

          

            result = TaskService.Instance.SetTaskDevice(taskid, deviceid);

            this.code = result.code;
            this.message = result.message;
            this.Data["data"] = result;
        }


        public void UpdateLocationStatus()
        {
            int taskid = this.GetFormValue("taskid",0);
            int locationid = this.GetFormValue("locationid", 0);
            string remark = this.GetFormValue("remark", "");
            ResultModel<TaskModel> result = new logic.Model.ResultModel<logic.Model.TaskModel>();
            

            int count = TaskService.Instance.UpdateLocationStatus(taskid, locationid, Constants.TASKSTATUS_FINISHED , remark);

            result.code = Constants.SUCCESS_CODE;
            result.message = "更新状态完成";
            this.code = result.code;
            this.message = result.message;
            this.Data["data"] = result;

        }

        public void ClearTaskDevice()
        {
            int taskid = this.GetFormValue("taskid", 0);

            TaskModel model = TaskService.Instance.GetTaskModel(taskid);
            if (model == null)
            {
                this.code = Constants.ERROR_CODE;
                this.message = "没有找到满足条件的任务信息";
                this.Data["data"] = null;
                return;
            }
            if (model.status != Constants.TASKSTATUS_INIT)
            {
                this.code = Constants.ERROR_CODE;
                this.message = "当前任务正在运行或已经完成，不能进行移除操作";
                this.Data["data"] = null;
                return;
            }

            int count = TaskService.Instance.ClearTaskDevice(taskid);
            this.code = Constants.SUCCESS_CODE;
            this.message = "移除成功";
            this.Data["data"] = null;
        }


        public void UpdateTaskStatusRun()
        {
            ResultModel<TaskModel> result = new logic.Model.ResultModel<logic.Model.TaskModel>();
            int taskid = this.GetFormValue("taskid", 0);
            if (taskid == 0)
            {
                result.code = Constants.ERROR_CODE;
                result.message = "任务id参数错误";
                this.code = result.code;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }

            int count =TaskService.Instance.UpdateTaskStatusRun(taskid, Constants.TASKSTATUS_RUNNING);
            if (count > 0)
            {
                result.code = Constants.SUCCESS_CODE;
                result.message = "任务状态更新成功";
                this.code = Constants.SUCCESS_CODE;
                this.message = result.message;
                this.Data["data"] = result;
                return;
            }
            else
            {
                result.code = Constants.ERROR_CODE;
                result.message = "任务状态更新失败";
                this.code = result.code;
                this.message = result.message;
                this.Data["data"] = result;
            }
        }
    }



    public class IBaseHandler : PageBaseHelper
    {

        public const int customerid = 1003;

        #region 初始变量
        private string _json = "";
        /// <summary>
        /// 
        /// </summary>
        public string outputJson { get { return _json; } set { _json = value; } }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public Dictionary<object, object> Data { get; set; }
        /// <summary>
        /// post操作类型 已经全部转换小写，前端不分大小写
        /// </summary>
        public string Action
        {
            get
            {
                string _action = this.GetFormValue("action", "").ToLower();
                if (string.IsNullOrEmpty(_action))
                    _action = this.GetQueryString("action", "").ToLower();
                return _action;
            }
        }

        private int _code = 0;
        /// <summary>
        /// 
        /// </summary>
        public int code { get { return _code; } set { _code = value; } }


        public string message { get; set; }


        #endregion



        /// <summary>
        /// 转换json格式
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public string GetJson(object value)
        {
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            };
            return JsonConvert.SerializeObject(value, timeConverter);
        }

    }
}