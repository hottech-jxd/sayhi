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
    public class UserSignUpDAL
    {

        /// <summary>
        /// 添加报名用户
        /// </summary>
        /// <param name="nonceCode"></param>
        /// <param name="redirectUrl"></param>
        public int AddSignUpUser(UserSignModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("IF NOT EXISTS (select top 1 id from Hot_SignUpUser where customerid=@customerid and openid=@openid and type=@type)  ");
            strSql.AppendLine(" BEGIN ");
            strSql.Append("insert into Hot_SignUpUser(");
            strSql.Append("customerid,name,mobile,address,openid,nickname,headimgurl,city,Province,type,content1,content2,content3,content4)");
            strSql.Append(" values (");
            strSql.Append("@customerid,@name,@mobile,@address,@openid,@nickname,@headimgurl,@city,@Province,@type,@content1,@content2,@content3,@content4)");
            strSql.AppendLine(" END ");
            strSql.AppendLine(" else ");
            strSql.AppendLine(" BEGIN ");
            strSql.Append(" update Hot_SignUpUser set name=@name,mobile=@mobile,address=@address ,nickname=@nickname,city=@city,Province=@Province,type=@type,content1=@content1,content2=@content2,content3=@content3,content4=@content4 where customerid=@customerid and openid=@openid and type=@type ");
            strSql.AppendLine(" END ");
            SqlParameter[] parameters = {
					new SqlParameter("@customerid", model.customerid),
					new SqlParameter("@name", model.name),
					new SqlParameter("@mobile", model.mobile),
                    new SqlParameter("@address", model.address),
                    new SqlParameter("@openid", model.openid),
                    new SqlParameter("@nickname", model.nickname),
                    new SqlParameter("@headimgurl", model.headimgurl),
                    new SqlParameter("@city", model.city),
                    new SqlParameter("@Province", model.Province),
                    new SqlParameter("@type", model.Type),
                    new SqlParameter("@content1", model.content1),
                    new SqlParameter("@content2", model.content2),
                    new SqlParameter("@content3", model.content3),
                    new SqlParameter("@content4", model.content4)
                                        };

            return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 获取当前报名用户信息
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public UserSignModel GetSignUpUserInfo(int customerId, string openid, int type = 0)
        {
            string sql = "select id,customerid,name,mobile,[address],openid,nickname,headimgurl,city,Province,createtime,type,content1,content2,content3,content4 from  Hot_SignUpUser where customerid=@customerid and openid=@openid and type=@type";
            SqlParameter[] parameters = {
					new SqlParameter("@customerid", customerId),
					new SqlParameter("@openid",openid),
                    new SqlParameter("@type",type)
             };
            UserSignModel model = new UserSignModel();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(sql, parameters))
            {
                model = DbHelperSQL.GetEntity<UserSignModel>(dr);
            }
            return model;
        }

        /// <summary>
        /// 获取报名数据
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<UserSignModel> GetUserSignList(int customerid, int pageIndex, int pageSize, out int recordCount, SignUserSearchWhere where)
        {
            string sql = @"select id,customerid,name,mobile,[address],openid,nickname,headimgurl,city,Province,createtime,content1,content2,content3,content4 from  Hot_SignUpUser where customerid=@customerid ";

            if (where != null)
            {
                if (!string.IsNullOrEmpty(where.key))
                    sql += string.Format(" and (name like '%{0}%' or name like '%{0}%'  or mobile like '%{0}%'   or nickname like '%{0}%')", where.key);


                if (!string.IsNullOrEmpty(where.pro))
                    sql += string.Format(" and Province='{0}' ", where.pro);
                if (!string.IsNullOrEmpty(where.city))
                    sql += string.Format(" and city='{0}' ", where.city);
                if (!string.IsNullOrEmpty(where.area))
                    sql += string.Format(" and address='{0}'", where.area);

            }
            SqlParameter[] parameters = {
					new SqlParameter("@customerid", customerid)
             };
            string querySql = DbHelperSQL.buildPageSql(pageIndex, pageSize, sql, "createtime", true);
            string recordCountSql = DbHelperSQL.buildRecordCountSql(sql);
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(recordCountSql, parameters));

            List<UserSignModel> lst = new List<UserSignModel>();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(querySql, parameters))
            {
                lst = DbHelperSQL.GetEntityList<UserSignModel>(dr);
            }
            return lst;
        }

        public DataTable GetUserSignUpList(int customerid)
        {
            string strSql = "select name AS '姓名',mobile AS '手机',Province AS '省份',city AS '城市',[address] AS '县区',content1 AS '店名',content3 AS '配送商',content4 AS '业务员',content2 AS '备注',createtime AS '申请时间' from  Hot_SignUpUser where customerid=@customerid ";
            SqlParameter[] parameters = {
					new SqlParameter("@customerid", customerid)
             };
            return DbHelperSQL.Query(strSql, parameters).Tables[0];
        }

    }
}
