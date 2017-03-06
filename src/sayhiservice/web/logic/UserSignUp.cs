using logic.DAL;
using logic.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace logic
{
    /// <summary>
    /// 报名
    /// </summary>
    public class UserSignUp
    {
        private static UserSignUp _Instance = null;
        private readonly UserSignUpDAL dal = new UserSignUpDAL();
        public UserSignUp() { }

        /// <summary>
        /// 单例出口
        /// </summary>
        public static UserSignUp Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new UserSignUp();
                }
                return _Instance;
            }
        }

        /// <summary>
        /// 获取当前报名用户信息
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public UserSignModel GetSignUpUserInfo(int customerId, string openid, int type = 0)
        {
            return dal.GetSignUpUserInfo(customerId, openid, type);
        }
        /// <summary>
        /// 添加报名用户
        /// </summary>
        /// <param name="nonceCode"></param>
        /// <param name="redirectUrl"></param>
        public int AddSignUpUser(UserSignModel model)
        {
            return dal.AddSignUpUser(model);
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
            return dal.GetUserSignList(customerid, pageIndex, pageSize, out recordCount, where);
        }

        public DataTable GetUserSignList(int customerid)
        {
            return dal.GetUserSignUpList(customerid);
        }
    }
}
