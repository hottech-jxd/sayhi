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
                    case "couponlist"://获取优惠券列表
                        couponList();
                        break;
                    case "helpuserlist"://查看所有助力用户
                        helpUserList();
                        break;
                    case "myhelpuserlist": //查看我的助力用户
                        myhelpUserList();
                        break;
                    case "shoplist": //门店列表
                        shopList();
                        break;
                    case "usersignlist"://报名用户
                        UserSignList();
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



        private void couponList()
        {
            int pageindex = this.GetFormValue("page", 1);//
            int pageSize = this.GetFormValue("pagesize", 20);
            int recordCount = 0;

            CouponSearchWhere where = new CouponSearchWhere();
            where.key = this.GetFormValue("name", "");
            where.id = this.GetFormValue("shopid", 0);
            where.useStatus = this.GetFormValue("type", -1);
            where.startTime = this.GetFormValue("txtStartTime", "");
            where.endTime = this.GetFormValue("txtEndTime", "");


            List<AttendUserModel> result = UserService.Instance.GetCouponList(customerid, pageindex, pageSize, out recordCount, where);
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


        private void helpUserList()
        {
            int pageindex = this.GetFormValue("page", 1);//
            int pageSize = this.GetFormValue("pagesize", 20);
            string name = this.GetFormValue("name", "");
            int recordCount = 0;
            List<HelpUserModel> result = UserService.Instance.GetAllhelpUserList(customerid, pageindex, pageSize, out recordCount);
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
        /// 获取我的助力用户
        /// </summary>
        private void myhelpUserList()
        {
            int pageindex = this.GetFormValue("page", 1);//
            int pageSize = this.GetFormValue("pagesize", 20);
            int userid = this.GetFormValue("userid", 0);
            int recordCount = 0;
            List<HelpUserModel> result = UserService.Instance.GethelpUserList(customerid, pageindex, pageSize, userid, out recordCount);
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
        /// 获取门店用户
        /// </summary>
        private void shopList()
        {
            int pageindex = this.GetFormValue("page", 1);//
            int pageSize = this.GetFormValue("pagesize", 20);
            int recordCount = 0;
            ShopSearchWhere where = new ShopSearchWhere();
            where.key = this.GetFormValue("name", "");
            where.area = this.GetFormValue("area", "");
            where.pro = this.GetFormValue("pro", "");
            where.city = this.GetFormValue("city", "");


            List<ShopUserModel> result = UserService.Instance.GetShopList(customerid, pageindex, pageSize, out recordCount, where);
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
        /// 获取报名用户
        /// </summary>
        private void UserSignList()
        {
            int pageindex = this.GetFormValue("page", 1);//
            int pageSize = this.GetFormValue("pagesize", 20);
           int _customerid = 1002;
            int recordCount = 0;
            SignUserSearchWhere where = new SignUserSearchWhere();
            where.key = this.GetFormValue("name", "");
            where.area = this.GetFormValue("area", "");
            where.pro = this.GetFormValue("pro", "");
            where.city = this.GetFormValue("city", "");

                      



            List<UserSignModel> result = UserSignUp.Instance.GetUserSignList(_customerid, pageindex, pageSize, out recordCount, where);
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


    }



    public class IBaseHandler : PageBaseHelper
    {

        public const int customerid = 1001;

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