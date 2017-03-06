using logic.DAL;
using logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace logic
{
    public class UserCarLogic
    {
        private static UserCarLogic _Instance = null;
        private readonly UserCarDAL dal = new UserCarDAL();
        public UserCarLogic() { }

        /// <summary>
        /// 单例出口
        /// </summary>
        public static UserCarLogic Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new UserCarLogic();
                }
                return _Instance;
            }
        }


        /// <summary>
        /// 添加报名用户
        /// </summary>
        public int AddUserCar(UserCarInfoModel model)
        {
            return dal.AddUserCar(model);
        }


        /// <summary>
        /// 修改审核状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool UpdateCheckStatus(int id, int status, string remark)
        {
            return dal.UpdateCheckStatus(id, status, remark);
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
            return dal.GetUserCarList(pageIndex, pageSize, out recordCount, where);
        }
        /// <summary>
        /// 添加客户资料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddCustomerModel(CustomerModel model)
        {
            return dal.AddCustomerModel(model);
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
            return dal.GetCusstomerList(pageIndex, pageSize, out recordCount, where);
        }
        /// <summary>
        /// 获取客户实体数据
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public CustomerModel GetCustomerModel(int UserId)
        {
            return dal.GetCustomerModel(UserId);
        }
    }
}
