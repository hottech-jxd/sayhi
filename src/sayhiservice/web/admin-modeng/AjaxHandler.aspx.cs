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
            this.Data["msg"] = this.msg;
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
                    case "usercarlist"://报名用户
                        UserCarList();
                        break;
                    case "getcustomerinfo":
                        getCustomerInfo();
                        break;
                    case "updatecheck":
                        updateCheck();
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
        /// 获取车辆申请列表
        /// </summary>
        private void UserCarList()
        {
            int type = this.GetFormValue("type", 0);//
            int pageindex = this.GetFormValue("page", 1);//
            int pageSize = this.GetFormValue("pagesize", 20);
            int status = this.GetFormValue("status", -2);
            int recordCount = 0;
            SearchWhere where = new SearchWhere();
            where.key = this.GetFormValue("name", "");
            where.type = type;
            where.Status = status;
            List<UserCarInfoModel> result = UserCarLogic.Instance.GetUserCarList(pageindex, pageSize, out recordCount, where);
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

        private void getCustomerInfo()
        {
            int id = this.GetFormValue("id", 0);//
            CustomerModel model = UserCarLogic.Instance.GetCustomerModel(id);
            if (model != null)
            {
                this.code = 1;
                this.Data["result"] = model;
            }
        }

        /// <summary>
        /// 审核
        /// </summary>
        private void updateCheck()
        {
            int id = this.GetFormValue("id", 0);//
            int type = this.GetFormValue("type", 0);//
            string remark = HttpUtility.UrlDecode(this.GetFormValue("remark", ""));//
            if (UserCarLogic.Instance.UpdateCheckStatus(id, type, remark))
            {
                this.code = 1;
            }

        }

    }



    public class IBaseHandler : PageBaseHelper
    {

        public const int customerid = 1004;

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


        public string msg { get; set; }


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