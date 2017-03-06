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

namespace msite
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
                    case "helpbargain":
                        HelpBargain();
                        break;
                    case "gethelpuserlist":
                        GethelpUserList();
                        break;
                    case "login":
                        login();
                        break;
                    case "signup":
                        signUp();
                        break;
                    case "modifypwd":
                        modifypwd();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("message:{0} ,StackTrace:{1}", ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// 朋友助力区间值
        /// </summary>
        public string sectionVal
        {
            get
            {
                return ConfigHelper.GetConfigString("sectionVal", "0|1");
            }
        }
        public string sectionVal2
        {
            get
            {
                return ConfigHelper.GetConfigString("sectionVal2", "0|1");
            }
        }
        /// <summary>
        /// 获取优惠券最大额度
        /// </summary>
        public decimal couponMaxValue
        {
            get
            {
                return Convert.ToDecimal(ConfigHelper.GetConfigString("couponMaxValue", "1000"));
            }
        }



        private string getTipContent(int customerid, int code)
        {
            return ConfigHelper.GetConfigString("tip_" + customerid.ToString() + code.ToString(), "");
        }
        /// <summary>
        /// 帮忙砍价
        /// </summary>
        private void HelpBargain()
        {
            string shareopenid = this.GetFormValue("shareopenid", "");//分享用户
            string openid = this.GetFormValue("openid", "");//当前用户
            int customerid = this.GetFormValue("customerid", 0);
            decimal money = 0;

            string[] arr = sectionVal.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            int minV = Convert.ToInt32(arr[0]);
            int maxV = Convert.ToInt32(arr[1]);

            string[] arr2 = sectionVal2.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            int minV2 = Convert.ToInt32(arr2[0]);
            int maxV2 = Convert.ToInt32(arr2[1]);
            try
            {
                AttendUserModel userModel = UserService.Instance.GetAttendUserInfo(customerid, shareopenid);
                if (userModel != null && userModel.isAttend == 1)
                {
                    bool isHelped = UserService.Instance.GethelpUser(customerid, openid, userModel.id);
                    if (isHelped)//已经助力过
                    {
                        this.code = 2;
                        msg = getTipContent(customerid, 2);
                        return;
                    }
                    //已经砍到最大额度了
                    if (couponMaxValue <= userModel.money)
                    {
                        this.code = 3;
                        msg = getTipContent(customerid, 3);
                        return;
                    }

                    if (userModel.useStatus == 1)
                    {
                        this.code = 5;
                        msg = getTipContent(customerid, 5);
                        return;
                    }


                    string endTime = ConfigHelper.GetConfigString("endTime", "2016-05-02");
                    DateTime outime;
                    DateTime.TryParse(endTime, out outime);
                    if (DateTime.Now > outime)//判断活动是否结束
                    {
                        this.code = 4;
                        msg = getTipContent(customerid, 4);
                        return;
                    }



                    Random random = new Random(Guid.NewGuid().GetHashCode());
                    if (userModel.money < 500)
                        money = Convert.ToDecimal((random.Next(minV, maxV) - random.NextDouble()));
                    else //用户助力到500元后，取另外一个金额区间
                        money = Convert.ToDecimal((random.Next(minV2, maxV2) - random.NextDouble()));

                    //当前看的价格加当前的价格，大于优惠券最大价格时
                    if (couponMaxValue <= (userModel.money + money))
                        money = couponMaxValue - userModel.money;

                    //更新优惠券额度
                    UserService.Instance.UpdateAttendUserMoney(customerid, shareopenid, money);
                    //添加砍价记录
                    UserService.Instance.AddHelpUser(customerid, userModel.id, openid, money);

                    code = 1;

                }
                else
                {
                    msg = getTipContent(customerid, 1);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                this.Data["money"] = money.ToString("f2");
            }
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        private void GethelpUserList()
        {
            string shareopenid = this.GetFormValue("shareopenid", "");//分享用户            
            int customerid = this.GetFormValue("customerid", 0);
            int pageIndex = this.GetFormValue("pageIndex", 0);
            int pageSize = this.GetFormValue("pageSize", 0);
            AttendUserModel userModel = UserService.Instance.GetAttendUserInfo(customerid, shareopenid);
            if (userModel != null)
            {
                List<HelpUserModel> lst = UserService.Instance.GethelpUserList(pageIndex, pageSize, customerid, userModel.id);
                this.Data["data"] = lst;
                this.Data["count"] = lst.Count();
                this.code = 1;
            }
            this.Data["pageIndex"] = pageIndex;

        }


        private void login()
        {
            string loginName = this.GetFormValue("loginName", "");
            string loginPwd = EncryptHelper.MD5_8(this.GetFormValue("loginPwd", ""));
            string openid = this.GetFormValue("openid", "");
            int customerid = this.GetFormValue("customerid", 0);

            int shopid = UserService.Instance.IsShopLogin(customerid, loginName, loginPwd);
            if (shopid > 0)
            {
                UserService.Instance.AddShopBind(customerid, shopid, openid);
                this.code = 1;
            }
            else
            {
                this.msg = "账号或密码不正确";
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        private void modifypwd()
        {
            string loginName = this.GetFormValue("loginName", "");
            string oldPwd = EncryptHelper.MD5_8(this.GetFormValue("oldPwd", ""));
            string newPwd = EncryptHelper.MD5_8(this.GetFormValue("newPwd", ""));
            int customerid = this.GetFormValue("customerid", 0);

            int shopid = UserService.Instance.IsShopLogin(customerid, loginName, oldPwd);
            if (shopid > 0)
            {
                UserService.Instance.ModifyShopLogin(customerid, shopid, newPwd);
                this.code = 1;
            }
            else
            {
                this.msg = "旧密码错误";
            }
        }

        private void signUp()
        {
            try
            {
                string name = this.GetFormValue("name", "");
                string mobile = this.GetFormValue("mobile", "");
                string province = this.GetFormValue("province", "");
                string city = this.GetFormValue("city", "");
                string county = this.GetFormValue("county", "");
                int customerid = this.GetFormValue("customerid", 0);


                WeixinOAuthUserInfoModel userInfo = UserService.Instance.GetUserInfo(customerid);

                UserSignModel model = new UserSignModel()
                {
                    name = name,
                    Province = province,
                    city = city,
                    address = county,
                    customerid = customerid,
                    mobile = mobile,
                    openid = userInfo.Openid,
                    nickname = userInfo.Nickname,
                    headimgurl = userInfo.Headimgurl,
                    Type = this.GetFormValue("type", 0),
                    content1 = this.GetFormValue("content1", ""),
                    content2 = this.GetFormValue("content2", ""),
                    content3 = this.GetFormValue("content3", ""),
                    content4 = this.GetFormValue("content4", ""),
                };

                if (UserSignUp.Instance.AddSignUpUser(model) > 0)
                {
                    this.code = 1;
                }
                else
                {
                    this.msg = "预约失败";
                }
            }
            catch (Exception ex)
            {
                this.msg = "预约失败";
                LogHelper.Error(string.Format("signUp--->>message:{0} ,StackTrace:{1}", ex.Message, ex.StackTrace));
            }

        }


    }

    public class IBaseHandler : PageBaseHelper
    {
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
        public string Action { get { return this.GetFormValue("action", "").ToLower(); } }

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