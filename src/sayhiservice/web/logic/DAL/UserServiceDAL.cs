using common;
using logic.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace logic.DAL
{
    public class UserServiceDAL
    {
        public string GetOAuthUrl(string nonceCode)
        {
            string sql = string.Format(@"select UOU_RedirectUrl from Hot_UserOAuthUrl WITH (nolock) where UOU_NonceCode='{0}'",
                nonceCode);
            object obj = DbHelperSQL.GetSingle(sql);
            if (obj != null)
            {
                return obj.ToString();
            }
            return "";
        }

        /// <summary>
        /// 添加授权后要跳转的网址
        /// </summary>
        /// <param name="nonceCode"></param>
        /// <param name="redirectUrl"></param>
        public void AddOAuthUrl(string nonceCode, string redirectUrl)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Hot_UserOAuthUrl(");
            strSql.Append("UOU_NonceCode,UOU_RedirectUrl,UOU_AddTime)");
            strSql.Append(" values (");
            strSql.Append("@UOU_NonceCode,@UOU_RedirectUrl,@UOU_AddTime)");
            SqlParameter[] parameters = {
					new SqlParameter("@UOU_NonceCode", SqlDbType.VarChar,10),
					new SqlParameter("@UOU_RedirectUrl", SqlDbType.VarChar,400),
					new SqlParameter("@UOU_AddTime", SqlDbType.DateTime)};
            parameters[0].Value = nonceCode;
            parameters[1].Value = redirectUrl;
            parameters[2].Value = DateTime.Now;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 删除临时的跳转地址
        /// </summary>
        /// <param name="nonceCode"></param>
        public void DelOAuthUrl(string nonceCode)
        {
            DbHelperSQL.ExecuteSql(string.Format("delete from Hot_UserOAuthUrl where UOU_NonceCode='{0}'", nonceCode));
        }




        /// <summary>
        /// 得到第一个注册过的会员
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="identification"></param>
        /// <returns></returns>
        public int GetTopOAuthedUserId(int customerId, string identification)
        {
            string sql = string.Format(@"select top 1 id from Hot_AttendUser where  openid='{0}' and customerid={1}", identification, customerId);
            object obj = DbHelperSQL.GetSingle(sql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// 修改用户参与状态
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="identification"></param>
        /// <returns></returns>
        public int UpdateAttendUserStatus(int customerId, string identification)
        {
            string sql = string.Format(@"update  Hot_AttendUser set isAttend=1,updateTime='{2}' where customerid={1} and openid='{0}'", identification, customerId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return DbHelperSQL.ExecuteSql(sql);
        }



        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddUserInfo(AttendUserModel model)
        {
            string strSql = string.Format(@"insert into Hot_AttendUser(customerid,openid,nickname,headimgurl,unionid,city,couponcode,money,isattend,defmoney) values(@customerid,@openid,@nickname,@headimgurl,@unionid,@city,@couponcode,@money,@isattend,@defmoney);select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@customerid", model.customerid),
					new SqlParameter("@openid",model.openid),
					new SqlParameter("@nickname", model.nickname),
                    new SqlParameter("@headimgurl", model.headimgurl),
                    new SqlParameter("@unionid",model.unionId),
                    new SqlParameter("@city", model.city),
                    new SqlParameter("@couponcode", model.couponCode),
                    new SqlParameter("@money", model.money),
                    new SqlParameter("@isattend", model.isAttend),                                              
                    new SqlParameter("@defmoney", model.defmoney)      
                                        };
            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="identification"></param>
        /// <returns></returns>
        public AttendUserModel GetAttendUserInfo(int customerId, string identification)
        {
            string sql = "select id,customerid,openid,nickname,headimgurl,unionid,city,couponcode,money,isattend,defmoney,useStatus,shopUserId from Hot_AttendUser where  openid=@openid and customerid=@customerid";
            SqlParameter[] parameters = {
					new SqlParameter("@customerid", customerId),
					new SqlParameter("@openid",identification)
             };
            AttendUserModel model = new AttendUserModel();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(sql, parameters))
            {
                model = DbHelperSQL.GetEntity<AttendUserModel>(dr);
            }
            return model;
        }

        /// <summary>
        /// 获取帮我助力的人数
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public int GetAttendActCount(int customerId, int UserId)
        {
            string sql = "select COUNT(1) from Hot_HelpUser where customerid=@customerid and UserId=@UserId";
            SqlParameter[] parameters = {
					new SqlParameter("@customerid", customerId),
                    new SqlParameter("@UserId", UserId)
             };
            object obj = DbHelperSQL.GetSingle(sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// 判断当前用户是否砍价过
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="helpOpenid"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool GethelpUser(int customerId, string helpOpenid, int UserId)
        {
            string sql = "select COUNT(1) from Hot_HelpUser where helpOpenid=@helpOpenid and customerid=@customerid  and UserId=@UserId";
            SqlParameter[] parameters = {
					new SqlParameter("@customerid", customerId),
					new SqlParameter("@helpOpenid",helpOpenid),
                    new SqlParameter("@UserId",UserId),
             };
            return Convert.ToInt32(DbHelperSQL.GetSingle(sql, parameters)) > 0;
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
            string sql = @"select h.*,a.headimgurl,a.nickName from Hot_HelpUser h
                           left join Hot_AttendUser a on a.openid=h.helpOpenid
                           where h.customerid=@customerid  and h.UserId=@UserId";
            SqlParameter[] parameters = {
					new SqlParameter("@customerid", customerId),
                    new SqlParameter("@UserId",UserId),
             };
            string querySql = DbHelperSQL.buildPageSql(pageIndex, pageSize, sql, "h.createtime");
            List<HelpUserModel> lst = new List<HelpUserModel>();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(querySql, parameters))
            {
                lst = DbHelperSQL.GetEntityList<HelpUserModel>(dr);
            }
            return lst;
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
            string sql = @"select h.*,a.headimgurl,a.nickName from Hot_HelpUser h
                           left join Hot_AttendUser a on a.openid=h.helpOpenid
                           where h.customerid=@customerid  and h.UserId=@UserId";
            SqlParameter[] parameters = {
					new SqlParameter("@customerid", customerId),
                    new SqlParameter("@UserId",UserId),
             };
            string querySql = DbHelperSQL.buildPageSql(pageIndex, pageSize, sql, "h.createtime");
            string recordCountSql = DbHelperSQL.buildRecordCountSql(sql);
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(recordCountSql, parameters));
            List<HelpUserModel> lst = new List<HelpUserModel>();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(querySql, parameters))
            {
                lst = DbHelperSQL.GetEntityList<HelpUserModel>(dr);
            }
            return lst;
        }

        /// <summary>
        /// 获取所有参与帮助的人数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="customerId"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public List<HelpUserModel> GetAllhelpUserList(int customerId, int pageIndex, int pageSize, out int recordCount)
        {
            string sql = @"select a.nickName,a.headimgurl,a.City from Hot_HelpUser h
                            left join Hot_AttendUser a on a.openid=h.helpOpenid
                            where h.customerid=@customerid
                            group by h.helpOpenid,a.nickName,a.headimgurl,a.City";

            SqlParameter[] parameters = {
					new SqlParameter("@customerid", customerId)
             };
            string querySql = DbHelperSQL.buildPageSql(pageIndex, pageSize, sql, "a.nickName");
            string recordCountSql = DbHelperSQL.buildRecordCountSql(sql);
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(recordCountSql, parameters));
            List<HelpUserModel> lst = new List<HelpUserModel>();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(querySql, parameters))
            {
                lst = DbHelperSQL.GetEntityList<HelpUserModel>(dr);
            }
            return lst;
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
            string sql = @"update  Hot_AttendUser set money=money+@money where customerid=@customerid and openid=@openid and isAttend=1 and useStatus=0";
            SqlParameter[] parameters = {
					new SqlParameter("@customerid", customerId),
					new SqlParameter("@openid",openid),
                    new SqlParameter("@money",money),
             };
            return DbHelperSQL.ExecuteSql(sql, parameters);
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
            string sql = "insert into Hot_HelpUser(customerid,UserId,money,helpOpenid) values(@customerid,@UserId,@money,@helpOpenid)";
            SqlParameter[] parameters = {
					new SqlParameter("@customerid", customerId),
					new SqlParameter("@UserId",userid),
                    new SqlParameter("@money",money),
                    new SqlParameter("@helpOpenid",helpOpenid)
             };
            return DbHelperSQL.ExecuteSql(sql, parameters);
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
            string sql = @"select * from Hot_AttendUser where isAttend=1 and customerid=@customerid";

            if (where != null)
            {
                if (!string.IsNullOrEmpty(where.key))
                    sql += string.Format(" and (couponCode='{0}' or nickName like '%{0}%')", where.key);

                if (where.id > 0)
                {
                    sql += string.Format(" and shopUserId={0} ", where.id);
                }

                if (where.useStatus != -1)
                {
                    sql += string.Format(" and useStatus={0} ", where.useStatus);
                }

                if (!string.IsNullOrEmpty(where.startTime))
                    sql += string.Format(" and CONVERT(nvarchar(10),updateTime,121)>='{0}' ", where.startTime);
                if (!string.IsNullOrEmpty(where.endTime))
                    sql += string.Format(" and CONVERT(nvarchar(10),updateTime,121)<='{0}' ", where.endTime);
            }

            SqlParameter[] parameters = {
					new SqlParameter("@customerid", customerid)
             };
            string querySql = DbHelperSQL.buildPageSql(pageIndex, pageSize, sql, "updateTime", true);
            string recordCountSql = DbHelperSQL.buildRecordCountSql(sql);
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(recordCountSql, parameters));
            List<AttendUserModel> lst = new List<AttendUserModel>();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(querySql, parameters))
            {
                lst = DbHelperSQL.GetEntityList<AttendUserModel>(dr);
            }
            return lst;
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
            string sql = @"select * from Hot_ShopUser where customerid=@customerid";

            if (where != null)
            {
                if (!string.IsNullOrEmpty(where.key))
                    sql += string.Format(" and (loginName like '%{0}%' or name like '%{0}%'  or shopName like '%{0}%')", where.key);

                if (!string.IsNullOrEmpty(where.area))
                    sql += string.Format(" and area='{0}' ", where.area);
                if (!string.IsNullOrEmpty(where.pro))
                    sql += string.Format(" and pro='{0}' ", where.pro);
                if (!string.IsNullOrEmpty(where.city))
                    sql += string.Format(" and city='{0}'", where.city);
            }


            SqlParameter[] parameters = {
					new SqlParameter("@customerid", customerid)
             };
            string querySql = DbHelperSQL.buildPageSql(pageIndex, pageSize, sql, "createtime", true);
            string recordCountSql = DbHelperSQL.buildRecordCountSql(sql);
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(recordCountSql, parameters));
            List<ShopUserModel> lst = new List<ShopUserModel>();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(querySql, parameters))
            {
                lst = DbHelperSQL.GetEntityList<ShopUserModel>(dr);
            }
            return lst;
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
            try
            {
                string sql = "select id from Hot_ShopUser where customerid=@customerid and loginName=@loginName and loginPassword=@loginPassword";
                SqlParameter[] parameters = {
					new SqlParameter("@customerid", customerid),
                    new SqlParameter("@loginName", loginName),
                    new SqlParameter("@loginPassword", loginPwd)
             };
                object obj = DbHelperSQL.GetSingle(sql, parameters);
                return obj != null ? Convert.ToInt32(obj) : 0;
            }
            catch (Exception)
            {
                return 0;
            }
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
            try
            {
                string sql = "update Hot_ShopUser set loginPassword=@loginPassword where  customerid=@customerid and id=@id";
                SqlParameter[] parameters = {
					new SqlParameter("@customerid", customerid),
                    new SqlParameter("@id", shopid),
                    new SqlParameter("@loginPassword", loginPwd)
             };
                object obj = DbHelperSQL.GetSingle(sql, parameters);
                return obj != null ? Convert.ToInt32(obj) : 0;
            }
            catch (Exception)
            {
                return 0;
            }
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
            string sql = @"IF NOT EXISTS (select top 1 id from Hot_ShopBind where customerid=@customerid and openid=@openid)  
                            BEGIN
 	                            insert into Hot_ShopBind(shopid,openid,customerid) values(@shopid,@openid,@customerid)
                            END
                            ";
            SqlParameter[] parameters = {
					new SqlParameter("@shopid", shopid),
                    new SqlParameter("@openid", openid),
                    new SqlParameter("@customerid", customerid)
             };
            return DbHelperSQL.ExecuteSql(sql, parameters);
        }

        /// <summary>
        /// 根据openid判断是否绑定门店
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public int GetShopId(int customerid, string openid)
        {
            try
            {
                string sql = "select shopid from Hot_ShopBind where customerid=@customerid and openid=@openid";
                SqlParameter[] parameters = {
                    new SqlParameter("@openid", openid),
                    new SqlParameter("@customerid", customerid)
                };
                object obj = DbHelperSQL.GetSingle(sql, parameters);
                return obj != null ? Convert.ToInt32(obj) : 0;
            }
            catch (Exception)
            {
                return 0;
            }
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
            string sql = "update Hot_AttendUser set useStatus=1,useTime=GETDATE(),shopUserId=@shopUserId where customerid=@customerid and useStatus=0 and isAttend=1 and couponCode=@couponCode";
            SqlParameter[] parameters = {
                    new SqlParameter("@shopUserId", shopid),
                    new SqlParameter("@customerid", customerid),
                    new SqlParameter("@couponCode", couponCode)
             };
            return DbHelperSQL.ExecuteSql(sql, parameters);
        }
        /// <summary>
        /// 根据优惠券码获取用户信息
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="couponCode"></param>
        /// <returns></returns>
        public AttendUserModel GetUserCouponInfo(int customerid,string couponCode) {
            string sql = "select top 1 id,customerid,openid,nickname,headimgurl,unionid,city,couponcode,money,isattend,defmoney,useStatus,shopUserId from Hot_AttendUser where  couponCode=@couponCode and customerid=@customerid";
            SqlParameter[] parameters = {
					new SqlParameter("@customerid", customerid),
					new SqlParameter("@couponCode",couponCode)
             };
            AttendUserModel model = new AttendUserModel();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(sql, parameters))
            {
                model = DbHelperSQL.GetEntity<AttendUserModel>(dr);
            }
            return model;
        }




        public int UpdateToken(int customerid, string token)
        {
            string sql = "update Hot_WxAccessToken set token=@token,gettime=GETDATE()  where customerid=@customerid";
            SqlParameter[] parameters = {                    
                    new SqlParameter("@customerid", customerid),
                    new SqlParameter("@token", token)
             };
            return DbHelperSQL.ExecuteSql(sql, parameters);
        }

        public int updateTicket(int customerid, string ticket)
        {
            string sql = "update Hot_WxTicket set ticket=@ticket,gettime=GETDATE()  where customerid=@customerid";
            SqlParameter[] parameters = {                    
                    new SqlParameter("@customerid", customerid),
                    new SqlParameter("@ticket", ticket)
             };
            return DbHelperSQL.ExecuteSql(sql, parameters);
        }

        public WxAccessTokenTicket GetToken(int customerid)
        {
            string sql = "select id,token as value,gettime from Hot_WxAccessToken where customerid=@customerid";
            SqlParameter[] parameters = {                    
                    new SqlParameter("@customerid", customerid)
             };
            WxAccessTokenTicket model = new WxAccessTokenTicket();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(sql, parameters))
            {
                model = DbHelperSQL.GetEntity<WxAccessTokenTicket>(dr);
            }
            return model;
        }

        public WxAccessTokenTicket GetTicket(int customerid)
        {
            string sql = "select id,ticket as value,gettime from Hot_WxTicket where customerid=@customerid";
            SqlParameter[] parameters = {                    
                    new SqlParameter("@customerid", customerid)
             };
            WxAccessTokenTicket model = new WxAccessTokenTicket();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(sql, parameters))
            {
                model = DbHelperSQL.GetEntity<WxAccessTokenTicket>(dr);
            }
            return model;
        }

    }
}
