using common;
using logic.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace logic.DAL
{
    public class UserCarDAL
    {
        /// <summary>
        /// 添加报名用户
        /// </summary>
        public int AddUserCar(UserCarInfoModel model)
        {
            string strSql = @"insert into MD_ApplyCarInsurance(OrderNo,Type,Usertoken,carInfo,carType,invoicePrice,InsuranceType,FristBeneficiary,agreement,WIE,City,CustomerId,Status,ApplyAge,LoanMoney,RepaymentPeriod)
                                                        values(@OrderNo,@Type,@Usertoken,@carInfo,@carType,@invoicePrice,@InsuranceType,@FristBeneficiary,@agreement,@WIE,@City,@CustomerId,@Status,@ApplyAge,@LoanMoney,@RepaymentPeriod);select @@IDENTITY";
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderNo", model.OrderNo),
                    new SqlParameter("@Type", model.Type),
                    new SqlParameter("@Usertoken", model.Usertoken),
                    new SqlParameter("@carInfo", model.carInfo),
                    new SqlParameter("@carType", model.carType),
                    new SqlParameter("@invoicePrice", model.invoicePrice),
                    new SqlParameter("@InsuranceType", model.InsuranceType),
                    new SqlParameter("@FristBeneficiary", model.FristBeneficiary),
                    new SqlParameter("@agreement", model.agreement),
                    new SqlParameter("@WIE", model.WIE),
                    new SqlParameter("@City", model.City),
                    new SqlParameter("@CustomerId", model.CustomerId),
                    new SqlParameter("@Status", model.Status),
                    new SqlParameter("@ApplyAge", model.ApplyAge),
                    new SqlParameter("@LoanMoney", model.LoanMoney),
                    new SqlParameter("@RepaymentPeriod", model.RepaymentPeriod)
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
        /// 获取车辆信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<UserCarInfoModel> GetUserCarList(int pageIndex, int pageSize, out int recordCount, SearchWhere where)
        {
            string sql = @"select id,OrderNo,Type,A.Usertoken,carInfo,carType,invoicePrice,InsuranceType,FristBeneficiary,agreement,WIE,City,CustomerId,Status,ApplyAge,LoanMoney,RepaymentPeriod,A.CreateTime,Remark,c.UserName as storeName  from  MD_ApplyCarInsurance A
            left join MD_CustomerList c on c.UserId=CustomerId
            where 1=1  ";

            if (where != null)
            {
                if (!string.IsNullOrEmpty(where.key))
                {
                    if (where.type == 1)
                        sql += string.Format(" and (carType like '%{0}%' or City like '%{0}%' or InsuranceType like '%{0}%' or agreement  like '%{0}%' or FristBeneficiary  like '%{0}%')", where.key);
                    else if (where.type == 0)
                        sql += string.Format(" and (carInfo like '%{0}%' or carType like '%{0}%'  or City like '%{0}%'   )", where.key);
                }

                if (where.type > -1)
                    sql += string.Format(" and Type={0} ", where.type);

                if (where.Status != -2)
                    sql += string.Format(" and Status={0} ", where.Status);


                if (!string.IsNullOrEmpty(where.token))
                    sql += string.Format(" and A.Usertoken='{0}' ", where.token);
            }
            string querySql = DbHelperSQL.buildPageSql(pageIndex, pageSize, sql, "A.createtime");
            string recordCountSql = DbHelperSQL.buildRecordCountSql(sql);
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(recordCountSql));

            List<UserCarInfoModel> lst = new List<UserCarInfoModel>();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(querySql))
            {
                lst = DbHelperSQL.GetEntityList<UserCarInfoModel>(dr);
            }
            return lst;
        }
        /// <summary>
        /// 添加客户资料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddCustomerModel(CustomerModel model)
        {
            StringBuilder strSql = new StringBuilder();
            if (model.UserId <= 0)
            {
                strSql.Append("insert into MD_CustomerList(");
                strSql.Append("UserToken,UserName,UserMobile,UserSex,UserIDCard,UserImg,UserType)");
                strSql.Append(" values (");
                strSql.Append("@UserToken,@UserName,@UserMobile,@UserSex,@UserIDCard,@UserImg,@UserType)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
                    new SqlParameter("@UserToken", model.UserToken),
                    new SqlParameter("@UserName",model.UserName),
                    new SqlParameter("@UserMobile",model.UserMobile),
                    new SqlParameter("@UserSex", model.UserSex),
                    new SqlParameter("@UserIDCard", model.UserIDCard),
                    new SqlParameter("@UserImg", model.UserImg),
                    new SqlParameter("@UserType", model.UserType)
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
            else
            {
                strSql.Append("update MD_CustomerList set UserName=@UserName,UserMobile=@UserMobile,UserImg=@UserImg where UserId=@UserId");
                SqlParameter[] parameters = {
                    new SqlParameter("@UserName",model.UserName),
                    new SqlParameter("@UserMobile",model.UserMobile),
                    new SqlParameter("@UserId", model.UserId),
                    new SqlParameter("@UserImg", model.UserImg)
                                        };
                int flag = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                return flag > 0 ? model.UserId : flag;
            }

        }

        /// <summary>
        /// 修改审核状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool UpdateCheckStatus(int id,int status,string remark) {
            string sql = "update MD_ApplyCarInsurance set Status=@Status,Remark=@Remark where id=@id";
            SqlParameter[] parameters = {
                    new SqlParameter("@Status",status),
                    new SqlParameter("@Remark",remark),
                    new SqlParameter("@id",id)
                                        };
            return DbHelperSQL.ExecuteSql(sql, parameters)>0;
        }


        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<CustomerModel> GetCusstomerList(int pageIndex, int pageSize, out int recordCount, SearchWhere where)
        {
            string sql = @"select UserId,UserToken,UserName,UserMobile,UserSex,UserIDCard,UserImg,CreateTime,UserType from  MD_CustomerList where 1=1  ";

            if (where != null)
            {
                if (!string.IsNullOrEmpty(where.key))
                    sql += string.Format(" and UserName like '%{0}%' ", where.key);

                if (!string.IsNullOrEmpty(where.token))
                    sql += string.Format(" and Usertoken='{0}' ", where.token);
            }
            string querySql = DbHelperSQL.buildPageSql(pageIndex, pageSize, sql, "CreateTime", true);
            string recordCountSql = DbHelperSQL.buildRecordCountSql(sql);
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(recordCountSql));

            List<CustomerModel> lst = new List<CustomerModel>();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(querySql))
            {
                lst = DbHelperSQL.GetEntityList<CustomerModel>(dr);
            }
            return lst;
        }
        /// <summary>
        /// 获取客户实体数据
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public CustomerModel GetCustomerModel(int UserId)
        {
            string sql = @"select UserId,UserToken,UserName,UserMobile,UserSex,UserIDCard,UserImg,CreateTime,UserType from  MD_CustomerList where UserId=@UserId  ";
            SqlParameter[] parameters = {
                    new SqlParameter("@UserId",UserId),
                                        };
            CustomerModel data = new CustomerModel();
            using (SqlDataReader dr = DbHelperSQL.ExecuteReader(sql, parameters))
            {
                data = DbHelperSQL.GetEntity<CustomerModel>(dr);
            }
            return data;
        }

    }
}
