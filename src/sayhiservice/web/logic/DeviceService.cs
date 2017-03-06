using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using logic.DAL;
using logic.Model;

namespace logic
{
    public  class DeviceService
    {
        private static DeviceService _Instance = null;
        private readonly DeviceDAL dal = new DeviceDAL();

        /// <summary>
        /// 单例出口
        /// </summary>
        public static DeviceService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new DeviceService();
                }
                return _Instance;
            }
        }


        public ResultModel<DeviceModel> AddDevice(string devicename, string deviceno, string osverion, string devicetype,String brand)
        {
            ResultModel<DeviceModel> result = new ResultModel<DeviceModel>();

            DeviceModel model = new DeviceModel();
            model.devicename = devicename;
            model.deviceno = deviceno;
            model.devicetype = devicetype;
            model.osversion = osverion;
            model.createtime = DateTime.Now;
            model.brand = brand;


            bool exist = dal.ExistByDeviceNo(deviceno);
            if (exist)
            {
                dal.UpdateDeviceByDeviceNo(model);

                result.code = Constants.SUCCESS_CODE;
                result.message = "设备更新成功";
                return result;
            }
            
           int count = dal.AddDevice(model);
           if (count > 0)
           {
               result.code = Constants.SUCCESS_CODE;
               result.message = "设备添加成功";
               return result;
           }
           else
           {
               result.code = Constants.ERROR_CODE;
               result.message = "设备添加失败";
               return result;
           }

        }


        public List<DeviceModel> DeviceList(int pageIndex, int pageSize, out int recordCount, DeviceWhere where)
        {
            return dal.GetDeviceList( pageIndex, pageSize, out recordCount, where);
        }

        public int DeleteDevice(int deviceid)
        {
            return dal.DeleteDevice(deviceid);
        }

        public DeviceModel GetDevice(int deviceid)
        {
            return dal.GetDeviceByDeviceId(deviceid);
        }

        public int UpdateDevice(int deviceid, string remark) { 
            return dal.UpdateDeviceRemark(deviceid,remark);
        }

    }
}
