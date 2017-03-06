using common;
using common.Caching;
using logic.DAL;
using logic.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace logic
{
    public class UserService
    {
        private static UserService _Instance = null;
        private readonly UserServiceDAL dal = new UserServiceDAL();
        public UserService() { }

        /// <summary>
        /// 单例出口
        /// </summary>
        public static UserService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new UserService();
                }
                return _Instance;
            }
        }


        /// <summary>
        /// 记录授权完后要去的地址
        /// </summary>
        /// <param name="nonceCode">随机码</param>
        /// <param name="redirectUrl">目标URL</param>
        public void AddOAuthUrl(string nonceCode, string redirectUrl)
        {
            CookieHelper.SetCookieVal("OAUTH_" + nonceCode, 3, redirectUrl);
            dal.AddOAuthUrl(nonceCode, redirectUrl);
        }
        /// <summary>
        /// 获取授权完后要去的网址
        /// </summary>
        /// <param name="nonceCode">随机码</param>
        /// <param name="afterDel">获取完后是否删除</param>
        /// <returns>目标URL</returns>
        public string GetOAuthUrl(string nonceCode, bool afterDel = true)
        {
            string url = dal.GetOAuthUrl(nonceCode);
            if (afterDel)
            {
                dal.DelOAuthUrl(nonceCode);
            }

            string datakey = "OAUTH_" + nonceCode;
            string _bakUrl = CookieHelper.GetCookieVal("OAUTH_" + nonceCode);

            if (!string.IsNullOrEmpty(_bakUrl) && url == "")
            {
                url = _bakUrl;
            }
            CookieHelper.DelCookieVal(datakey);
            return url;
        }
        /// <summary>
        /// 得到第一个注册过的会员
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="identification"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetTopOAuthedUserId(int customerId, string identification)
        {
            return dal.GetTopOAuthedUserId(customerId, identification);
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="oauth"></param>
        /// <returns></returns>
        public int RegisterUser(int customerId, WeixinOAuthUserInfoModel oauth)
        {
            AttendUserModel model = new AttendUserModel()
            {
                customerid = customerId,
                headimgurl = oauth.Headimgurl,
                openid = oauth.Openid,
                nickname = oauth.Nickname,
                city = oauth.City,
                isAttend = 0,
                unionId = oauth.UnionID,
                couponCode = StringHelper.RandomNo(new Random(Guid.NewGuid().GetHashCode()), 16)
            };
            string couponValue = ConfigHelper.GetConfigString("couponValue", "0");
            if (!string.IsNullOrEmpty(couponValue))
                model.money = Convert.ToDecimal(couponValue);

            model.defmoney = model.money;

            return dal.AddUserInfo(model);
        }

        /// <summary>
        /// 修改用户参与状态
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="identification"></param>
        /// <returns></returns>
        public int UpdateAttendUserStatus(int customerId, string identification)
        {
            return dal.UpdateAttendUserStatus(customerId, identification);
        }
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="identification"></param>
        /// <returns></returns>
        public AttendUserModel GetAttendUserInfo(int customerId, string identification)
        {
            return dal.GetAttendUserInfo(customerId, identification);
        }



        /// <summary>
        /// 获取参与活动人数(是指发起数量)
        /// 默认每隔5分钟，数据更新一下
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="seconds">每隔seconds分钟，更新一次</param>
        /// <returns></returns>
        public int GetAttendActCount(int customerId, int userId, int seconds = 5)
        {
            string key = string.Format("at_act_c_{1}_{0}", customerId, userId);
            object obj = WebCacheHelper.Get(key);
            if (obj != null)
            {
                return Convert.ToInt32(obj);
            }
            int Invokecount = dal.GetAttendActCount(customerId, userId);
            WebCacheHelper.Insert(key, Invokecount, new TimeSpan(seconds * 1000 * 60));
            return Invokecount;
        }

        /// <summary>
        /// 判断当前用户是否砍价过
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="helpOpenid"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool GethelpUser(int customerId, string helpOpenid, int UserId, int seconds = 60)
        {
            string key = string.Format("help_{1}_c_{0}", customerId, UserId);
            object obj = WebCacheHelper.Get(key);
            if (obj != null)
            {
                return Convert.ToBoolean(obj);
            }
            bool val = dal.GethelpUser(customerId, helpOpenid, UserId);
            WebCacheHelper.Insert(key, val, new TimeSpan(seconds * 1000 * 60));
            return val;

        }

        /// <summary>
        /// 添加砍价用户
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="userid"></param>
        /// <param name="helpOpenid"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public int AddHelpUser(int customerId, int userid, string helpOpenid, decimal money)
        {
            int flag = dal.AddHelpUser(customerId, userid, helpOpenid, money);
            if (flag > 0)
            {
                string key = string.Format("help_{1}_c_{0}", customerId, userid);
                bool val = dal.GethelpUser(customerId, helpOpenid, userid);
                WebCacheHelper.Insert(key, val, new TimeSpan(60 * 1000 * 60));
            }
            return flag;
        }

        /// <summary>
        /// 更新用户优惠券价格
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="openid"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public int UpdateAttendUserMoney(int customerId, string openid, decimal money)
        {
            return dal.UpdateAttendUserMoney(customerId, openid, money);
        }


        /// <summary>
        /// 获取帮忙砍价的人数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="customerId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<HelpUserModel> GethelpUserList(int pageIndex, int pageSize, int customerId, int UserId)
        {
            return dal.GethelpUserList(pageIndex, pageSize, customerId, UserId);
        }
        /// <summary>
        /// 获取帮我助力的用户
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="UserId"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public List<HelpUserModel> GethelpUserList(int customerId, int pageIndex, int pageSize, int UserId, out int recordCount)
        {
            return dal.GethelpUserList(customerId, pageIndex, pageSize, UserId, out recordCount);
        }

        /// <summary>
        /// 获取优惠券列表
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<AttendUserModel> GetCouponList(int customerid, int pageIndex, int pageSize, out int recordCount, CouponSearchWhere where)
        {
            return dal.GetCouponList(customerid, pageIndex, pageSize, out recordCount, where);
        }

        /// <summary>
        /// 获取所有参与帮助的人数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="customerId"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public List<HelpUserModel> GetAllhelpUserList(int pageIndex, int pageSize, int customerId, out int recordCount)
        {
            return dal.GetAllhelpUserList(pageIndex, pageSize, customerId, out recordCount);
        }


        /// <summary>
        /// 获取门店数据
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ShopUserModel> GetShopList(int customerid, int pageIndex, int pageSize, out int recordCount, ShopSearchWhere where)
        {
            return dal.GetShopList(customerid, pageIndex, pageSize, out recordCount, where);
        }


        /// <summary>
        /// 门店账号检查登录，并返回账号id
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        public int IsShopLogin(int customerid, string loginName, string loginPwd)
        {
            return dal.IsShopLogin(customerid, loginName, loginPwd);
        }

        /// <summary>
        /// 添加门店绑定
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="shopid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public int AddShopBind(int customerid, int shopid, string openid)
        {
            return dal.AddShopBind(customerid, shopid, openid);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="shopid"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        public int ModifyShopLogin(int customerid, int shopid, string loginPwd)
        {
            return dal.ModifyShopLogin(customerid, shopid, loginPwd);
        }

        /// <summary>
        /// 根据openid判断是否绑定门店
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public int GetShopId(int customerid, string openid)
        {
            return dal.GetShopId(customerid, openid);
        }
        /// <summary>
        /// 使用优惠券
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="shopid"></param>
        /// <param name="couponCode"></param>
        /// <returns></returns>
        public int ShopUseCoupon(int customerid, int shopid, string couponCode)
        {
            return dal.ShopUseCoupon(customerid, shopid, couponCode);
        }

        /// <summary>
        /// 根据优惠券码获取用户信息
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="couponCode"></param>
        /// <returns></returns>
        public AttendUserModel GetUserCouponInfo(int customerid, string couponCode)
        {
            return dal.GetUserCouponInfo(customerid, couponCode);
        }






        public int UpdateToken(int customerid, string token)
        {
            return dal.UpdateToken(customerid, token);
        }
        public int updateTicket(int customerid, string ticket)
        {
            return dal.updateTicket(customerid, ticket);
        }
        public WxAccessTokenTicket GetToken(int customerid)
        {
            return dal.GetToken(customerid);
        }
        public WxAccessTokenTicket GetTicket(int customerid)
        {
            return dal.GetTicket(customerid);
        }






        public const string ENCRYPTKEY = "lechen20";
        /// <summary>
        /// 获取微信授权用户信息
        /// </summary>
        /// <param name="customerId">商户Id</param>
        /// <returns></returns>
        public WeixinOAuthUserInfoModel GetUserInfo(int customerId)
        {
            string keyUserinfo = this.GetUserinfoDataKey(customerId);
            string encryptedUserInfo = CookieHelper.GetCookieVal(keyUserinfo);
            if (string.IsNullOrEmpty(encryptedUserInfo))
            {
                //尝试从session中读取
                if (HttpContext.Current.Session[keyUserinfo] != null)
                {
                    WeixinOAuthUserInfoModel seModel = HttpContext.Current.Session[keyUserinfo] as WeixinOAuthUserInfoModel;
                    return seModel;
                }
                return null;
            }
            try
            {
                WeixinOAuthUserInfoModel model = JsonConvert.DeserializeObject<WeixinOAuthUserInfoModel>(EncryptHelper.Decrypt(encryptedUserInfo, ENCRYPTKEY));
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("WeixinOAuthUserDataProvider->GetUserInfo发生异常：{0}", ex.Message));
            }
            return null;
        }
        private string GetUserinfoDataKey(int customerId)
        {
            return "wxoauth_uinfo_" + customerId;
        }


        /// <summary>
        /// 保存微信授权用户信息
        /// </summary>
        /// <param name="customerId">商户Id</param>
        /// <param name="model"></param>
        public void SetUserInfo(int customerId, WeixinOAuthUserInfoModel model)
        {
            string json = JsonConvert.SerializeObject(model);
            string jsonEncrypt = EncryptHelper.Encrypt(json, ENCRYPTKEY);
            CookieHelper.SetCookieValByCurrentDomain(this.GetUserinfoDataKey(customerId), 1, jsonEncrypt);

            //双保险，session也存储
            HttpContext.Current.Session[this.GetUserinfoDataKey(customerId)] = model;
        }

















    }
}
